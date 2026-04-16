using System;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppEntity.Enums;

namespace QuantityMeasurementAppBusiness.Implementations
{
    /// <summary>
    /// Measurable adapter for length units.
    /// Base unit: METERS (all values convert to/from meters internally).
    /// </summary>
    public class LengthMeasurable : IMeasurable
    {
        private readonly LengthUnit _unit;

        public LengthMeasurable(LengthUnit unit)
        {
            _unit = unit;
        }

        // ── Conversion factors (unit → meters) ───────────────────────────────

        public double GetConversionFactor()
        {
            switch (_unit)
            {
                case LengthUnit.FEET:        return 0.3048;
                case LengthUnit.INCHES:      return 0.0254;
                case LengthUnit.YARDS:       return 0.9144;
                case LengthUnit.CENTIMETERS: return 0.01;
                default:
                    throw new ArgumentException(
                        $"Unknown LengthUnit: {_unit}");
            }
        }

        // ── IMeasurable ───────────────────────────────────────────────────────

        public double ConvertToBaseUnit(double value)
        {
            return value * GetConversionFactor();   // → meters
        }

        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / GetConversionFactor(); // meters → this unit
        }

        public string GetUnitName()        => _unit.ToString();
        public string GetMeasurementType() => "LENGTH";
        public bool   SupportsArithmetic() => true;

        public void ValidateOperationSupport(string operation)
        {
            // All arithmetic operations are supported for length — no-op.
        }

        public override string ToString() => $"LengthMeasurable({_unit})";
    }
}