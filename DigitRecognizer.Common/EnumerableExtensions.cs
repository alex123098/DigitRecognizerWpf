using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DigitRecognizer.Common.EnumerableExtensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Action<T> action)
        {
            Guard.NotNull(sequence, nameof(sequence));
            Guard.NotNull(action,   nameof(action));

            foreach (var item in sequence)
            {
                action(item);
            }
        }

        public static void ForEach<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Action<int, T> action)
        {
            Guard.NotNull(sequence, nameof(sequence));
            Guard.NotNull(action, nameof(action));

            var index = 0;
            foreach (var item in sequence)
            {
                action(index++, item);
            }
        }
    }
}
