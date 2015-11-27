using System;

namespace DigitRecognizer.Common
{
    public static class Guard
    {
        public static void NotNull<T>(T @this, string parameterName) where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
