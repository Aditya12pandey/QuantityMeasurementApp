using System;

namespace QuantityService.Business;

public class QuantityException : Exception
{
    public QuantityException(string message) : base(message) { }
    public QuantityException(string message, Exception inner) : base(message, inner) { }
}
