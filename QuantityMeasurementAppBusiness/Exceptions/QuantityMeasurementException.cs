using System;

namespace QuantityMeasurementAppBusiness.Exceptions
{
    public class QuantityMeasurementException : Exception
    {
        // Custom exception for all quantity measurement domain errors
        public QuantityMeasurementException(string message)
            : base(message) { }

        // Overloaded constructor for chaining inner exceptions
        public QuantityMeasurementException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}