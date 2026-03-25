using System;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppEntity.Enums;

namespace QuantityMeasurementAppBusiness.Implementations
{
    /// <summary>
    /// Measurable adapter for volume units.
    /// Base unit: LITRES (all values convert to/from litres internally).
    /// </summary>
    public class VolumeMeasurable : IMeasurable
    {
        private readonly VolumeUnit _unit;

        public VolumeMeasurable(VolumeUnit unit)
        {
            _unit = unit;
        }

        // ── Conversion factors (unit → litres) ───────────────────────────────

        public double GetConversionFactor()
        {
            switch (_unit)
            {
                case VolumeUnit.LITRE:      return 1.0;
                case VolumeUnit.MILLILITRE: return 0.001;
                case VolumeUnit.GALLON:     return 3.785411784; // US liquid gallon
                default:
                    throw new ArgumentException(
                        $"Unknown VolumeUnit: {_unit}");
            }
        }

        // ── IMeasurable ───────────────────────────────────────────────────────

        public double ConvertToBaseUnit(double value)
        {
            return value * GetConversionFactor();   // → litres
        }

        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / GetConversionFactor(); // litres → this unit
        }

        public string GetUnitName()        => _unit.ToString();
        public string GetMeasurementType() => "VOLUME";
        public bool   SupportsArithmetic() => true;

        public void ValidateOperationSupport(string operation)
        {
            // All arithmetic operations are supported for volume — no-op.
        }

        public override string ToString() => $"VolumeMeasurable({_unit})";
    }
}