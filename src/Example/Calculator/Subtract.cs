using Calculator.Interfaces;

namespace Calculator
{
    public class Subtract
        : ISubtract
    {
        public int Execute(int a,
                           int b)
        {
            return a - b;
        }
    }
}