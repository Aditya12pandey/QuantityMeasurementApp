using System;

namespace QuantityMeasurementApp
{
    public class SupportsArithmeticImpl : ISupportsArithmetic
    {
        private readonly Func<bool> _func;

        public SupportsArithmeticImpl(Func<bool> func)
        {
            _func = func;
        }

        public bool IsSupported()
        {
            return _func();
        }
    }
}