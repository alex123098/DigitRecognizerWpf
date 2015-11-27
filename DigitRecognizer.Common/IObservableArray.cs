using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DigitRecognizer.Infrastructure
{
    public interface IObservableArray<T> : INotifyCollectionChanged, INotifyPropertyChanged, IEnumerable<T>
    {
        T this [int index] { get; set; }
        int Length { get; }
    }
}
