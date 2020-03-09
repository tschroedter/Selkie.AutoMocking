using JetBrains.Annotations;

namespace Calculator.Interfaces
{
    public interface ICalculator
    {
        [UsedImplicitly]
        int Add(int      a, int b);

        [UsedImplicitly]
        int Subtract(int a, int b);
    }
}