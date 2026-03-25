using System;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppEntity.Enums;

namespace QuantityMeasurementAppBusiness.Implementations
{
    /// <summary>
    /// Measurable adapter for weight units.
    /// Base unit: KILOGRAMS (all values convert to/from kilograms internally).
    /// </summary>
    public class WeightMeasurable : IMeasurable
    {
        private readonly WeightUnit _unit;

        public WeightMeasurable(WeightUnit unit)
        {
            _unit = unit;
        }

        // ── Conversion factors (unit → kilograms) ────────────────────────────

        public double GetConversionFactor()
        {
            switch (_unit)
            {
                case WeightUnit.KILOGRAM: return 1.0;
                case WeightUnit.GRAM:     return 0.001;
                case WeightUnit.POUND:    return 0.45359237;
                default:
                    throw new ArgumentException(
                        $"Unknown WeightUnit: {_unit}");
            }
        }

        // ── IMeasurable ───────────────────────────────────────────────────────

        public double ConvertToBaseUnit(double value)
        {
            return value * GetConversionFactor();   // → kilograms
        }

        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / GetConversionFactor(); // kilograms → this unit
        }

        public string GetUnitName()        => _unit.ToString();
        public string GetMeasurementType() => "WEIGHT";
        public bool   SupportsArithmetic() => true;

        public void ValidateOperationSupport(string operation)
        {
            // All arithmetic operations are supported for weight — no-op.
        }

        public override string ToString() => $"WeightMeasurable({_unit})";
    }
}   