namespace DigitRecognizer.RecognizeLibrary

open System
open DigitRecognizer.Infrastructure

type Recognizer() = 
    interface IDigitRecognizer with
        member x.Recognize (image : byte IObservableArray) =
            image
            |> Seq.toArray
            |> Recognizer.recognize
            |> Async.StartAsTask

type LearnService() =
    interface IDigitLearnService with
        member x.Learn (image : byte IObservableArray, digit : byte) = 
            image
            |> Seq.toArray
            |> Recognizer.learn digit
            |> Async.Start
