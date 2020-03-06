﻿using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Selkie.AutoMocking
{
    public static class Guard
    {
        private const string ValueCannotBeNullOrEmpty = "Value cannot be null or empty";
        private const string ValueCannotBeWhitespace  = "Value cannot be null, empty or whitespace";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ArgumentNotEmptyOrWhitespace([NotNull] object parameter,
                                                        [NotNull] string parameterName)
        {
            ArgumentNotNullOrEmpty(parameter,
                                   parameterName);

            if (!(parameter is string text) ||
                !string.IsNullOrWhiteSpace(text))
                return;

            var message = string.Format(CultureInfo.InvariantCulture,
                                        ValueCannotBeWhitespace);

            throw new ArgumentException(message,
                                        parameterName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ArgumentNotNull([NotNull] object parameter,
                                           [NotNull] string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ArgumentNotNullOrEmpty([NotNull] object parameter,
                                                  [NotNull] string parameterName)
        {
            ArgumentNotNull(parameter,
                            parameterName);

            if (!(parameter is string text) ||
                text.Length != 0)
                return;

            var message = string.Format(CultureInfo.InvariantCulture,
                                        ValueCannotBeNullOrEmpty);

            throw new ArgumentException(message,
                                        parameterName);
        }
    }
}