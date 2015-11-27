using DigitRecognizer.Infrastructure;

namespace DigitRecognizer.Common.ObservableExtensions
{
    public static class ObservableExtensions
    {
        public static IObservableArray<T> ToObservableArray<T>(this T[] array) => new ObservableArray<T>(array);
    }
}
