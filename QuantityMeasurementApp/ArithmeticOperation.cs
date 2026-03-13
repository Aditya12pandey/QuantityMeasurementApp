using System;

namespace QuantityMeasurementApp
{
    public enum ArithmeticOperation
    {
        ADD,
        SUBTRACT,
        DIVIDE,
        MULTIPLY
    }

    public static class ArithmeticOperationExtensions
    {
        public static double Compute(this ArithmeticOperation operation, double a, double b)
        {
            switch (operation)
            {
                case ArithmeticOperation.ADD:
                    return a + b;

                case ArithmeticOperation.SUBTRACT:
                    return a - b;

                case ArithmeticOperation.DIVIDE:
                    if (Math.Abs(b) < 1e-10)
                        throw new ArithmeticException("Division by zero is not allowed");
                    return a / b;

                case ArithmeticOperation.MULTIPLY:
                    return a * b;

                default:
                    throw new ArgumentException("Unsupported arithmetic operation");
            }
        }
    }
}