using QuantityMeasurementAppEntity.DTOs;

namespace QuantityMeasurementAppBusiness
{
    public interface IQuantityMeasurementService
    {
        // Compare two quantities after base-unit conversion
        bool Compare(QuantityDTO quantity1, QuantityDTO quantity2);

        // Convert quantity to the unit specified in targetUnit
        QuantityDTO Convert(QuantityDTO quantity, QuantityDTO targetUnit);

        // Add two quantities; result expressed in targetUnit
        QuantityDTO Add(QuantityDTO quantity1, QuantityDTO quantity2,
                        QuantityDTO targetUnit);

        // Subtract quantity2 from quantity1; result expressed in targetUnit
        QuantityDTO Subtract(QuantityDTO quantity1, QuantityDTO quantity2,
                             QuantityDTO targetUnit);

        // Divide quantity1 by quantity2; returns dimensionless ratio
        double Divide(QuantityDTO quantity1, QuantityDTO quantity2);
    }
}