using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DigitRecognizer.Common;
using DigitRecognizer.Common.Annotations;

namespace DigitRecognizer.Infrastructure
{
    internal class ObservableArray<T> : IObservableArray<T>
    {
        private readonly T[] _array;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableArray(T[] array)
        {
            Guard.NotNull(array, nameof(array));
            _array = array;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _array.OfType<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get { return _array[index]; }
            set
            {
                var oldValue = _array[index];
                _array[index] = value;
                OnPropertyChanged();
                OnCollectionChanged(index, oldValue, value);
            }
        }

        public int Length => _array.Length;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnCollectionChanged(int index, T oldValue, T newValue)
        {
            var valueChanged = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,
                new[] {newValue},
                new[] {oldValue},
                index);
            CollectionChanged?.Invoke(this, valueChanged);
        }
    }
}