using Calculator.Interfaces;
using JetBrains.Annotations;

namespace Calculator
{
    public class Calculator
        : ICalculator
    {
        private readonly IAdd      _add;
        private readonly ISubtract _subtract;

        public Calculator([NotNull] IAdd      add,
                          [NotNull] ISubtract subtract)
        {
            Guard.ArgumentNotNull(add,
                                  nameof(add));
            Guard.ArgumentNotNull(subtract,
                                  nameof(subtract));

            _add      = add;
            _subtract = subtract;
        }

        public int Add(int a,
                       int b)
        {
            return _add.Execute(a,
                                b);
        }

        public int Subtract(int a,
                            int b)
        {
            return _subtract.Execute(a,
                                     b);
        }
    }
}