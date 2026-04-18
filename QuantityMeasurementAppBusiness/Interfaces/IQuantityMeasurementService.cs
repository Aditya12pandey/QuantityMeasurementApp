using QuantityMeasurementAppEntity.DTOs;

namespace QuantityMeasurementAppBusiness
{
    public interface IQuantityMeasurementService
    {
        // Compare two quantities after base-unit conversion
        bool Compare(string userId, QuantityDTO quantity1, QuantityDTO quantity2);
        QuantityDTO Convert(string userId, QuantityDTO quantity, QuantityDTO targetUnit);
        QuantityDTO Add(string userId, QuantityDTO quantity1, QuantityDTO quantity2, QuantityDTO targetUnit);
        QuantityDTO Subtract(string userId, QuantityDTO quantity1, QuantityDTO quantity2, QuantityDTO targetUnit);
        double Divide(string userId, QuantityDTO quantity1, QuantityDTO quantity2);
    }
}