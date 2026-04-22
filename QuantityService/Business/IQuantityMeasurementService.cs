using QuantityService.Models;

namespace QuantityService.Business;

public interface IQuantityMeasurementService
{
    bool Compare(QuantityDTO dto1, QuantityDTO dto2);
    QuantityDTO Convert(QuantityDTO quantity, string targetUnit);
    QuantityDTO Add(QuantityDTO dto1, QuantityDTO dto2, string targetUnit);
    QuantityDTO Subtract(QuantityDTO dto1, QuantityDTO dto2, string targetUnit);
    double Divide(QuantityDTO dto1, QuantityDTO dto2);
}
