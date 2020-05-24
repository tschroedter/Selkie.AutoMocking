using System;
using Selkie.AutoMocking.Interfaces;

namespace Selkie.AutoMocking
{
    /// <inheritdoc />
    public class ArgumentNullExceptionFinder : IArgumentNullExceptionFinder
    {
        /// <inheritdoc />
        public bool TryFindArgumentNullException(Exception                 exception,
                                                 out ArgumentNullException argumentNullException,
                                                 int                       maxDepth = 10)
        {
            Guard.ArgumentNotNull(exception,
                                  nameof(exception));

            if (maxDepth <= 0)
                throw new ArgumentException($"{maxDepth} must be > 0",
                                            nameof(maxDepth));

            if (exception is ArgumentNullException isArgumentNullException)
            {
                argumentNullException = isArgumentNullException;

                return true;
            }

            argumentNullException = null;

            var current = exception;
            var last    = current;
            var count   = 0;

            while (current != null &&
                   count++ < maxDepth)
            {
                last    = current;
                current = current.InnerException;
            }

            if (!(last is ArgumentException argumentException) ||
                !argumentException.Message.StartsWith("Value cannot be null."))
                return false;

            Console.WriteLine("Creating ArgumentNullException with "             +
                              $"parameter name '{argumentException.ParamName}' " +
                              $"and message '{argumentException.Message}'.");

            argumentNullException = new ArgumentNullException(argumentException.ParamName);

            return true;
        }
    }
}