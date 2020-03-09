using Calculator.Interfaces;

namespace Calculator
{
    public class Add
        : IAdd
    {
        public int Execute(int a,
                           int b)
        {
            return a + b;
        }
    }
}