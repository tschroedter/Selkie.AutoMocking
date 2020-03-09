using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Calculator
{
    public static class Guard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ArgumentNotNull([NotNull] object parameter,
                                           [NotNull] string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }
    }
}