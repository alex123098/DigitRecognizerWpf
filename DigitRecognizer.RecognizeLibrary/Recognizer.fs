module Recognizer

open System
open System.IO
open Microsoft.FSharp.Control.CommonExtensions
open DigitRecognizer.Common

type Bitmap = byte array

type Sample =
    {
        Value : byte
        Bitmap : Bitmap
    }

let storagePath = "samples.csv"

let merge (arr1 : 'a array) (arr2 : 'a array) =
    seq {
        let length = min arr1.Length arr2.Length
        for i in 0..(length - 1) do
            yield (arr1.[i], arr2.[i])
    }

let diff (bitmap1 : Bitmap) (bitmap2 : Bitmap) =
    bitmap1
    |> merge bitmap2
    |> Seq.map (fun (l, r) -> float(l - r) ** 2.0)
    |> Seq.sum
    |> sqrt

let guesDigit (bitmap : Bitmap) (samples : Sample seq) =
    samples
    |> Seq.sortBy (fun sample -> diff sample.Bitmap bitmap)
    |> Seq.take 5
    |> Seq.countBy (fun element -> element.Value)
    |> Seq.maxBy (fun (value, count) -> count)
    |> fst

let skipHeader (lines: 'a array) = lines.[1..]

let parseDigits (str : string) =
    let data = str.Split([| ',' |], StringSplitOptions.RemoveEmptyEntries)
    let digit = data.[0] |> byte
    let bitmap = data.[1..] |> Array.map byte
    { Value = digit; Bitmap = bitmap; }

let splitLines (str : string) = str.Split([|Environment.NewLine|], StringSplitOptions.RemoveEmptyEntries)

let loadSamplesString path =
    async {
        use reader = File.OpenText(path)
        let! lines = reader.ReadToEndAsync() |> Async.AwaitTask
        return lines
    }

let recognize bitmap =
    async {
        let! lines = loadSamplesString storagePath 
        return 
            lines
            |> splitLines
            |> skipHeader
            |> Seq.map (fun line -> parseDigits line)
            |> guesDigit bitmap
    }

let learn (digit : byte) (bitmap : Bitmap) =
    let stringToWrite = String.Join(",", digit::(bitmap |> Array.toList))
    async {
        use writer = new StreamWriter(storagePath, append=true)
        do! writer.WriteLineAsync(stringToWrite) |> Async.AwaitTask
    }