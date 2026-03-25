using System;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppEntity.Enums;

namespace QuantityMeasurementAppBusiness.Implementations
{
    /// <summary>
    /// Measurable adapter for temperature units.
    /// Base unit: CELSIUS.
    ///
    /// Temperature conversion is NON-LINEAR (uses offset formula, not a
    /// simple multiplication factor), so GetConversionFactor() is not
    /// meaningful here — ConvertToBaseUnit / ConvertFromBaseUnit override
    /// the linear pattern used by other measurables.
    ///
    /// Arithmetic (ADD, SUBTRACT, DIVIDE) is NOT supported for temperature
    /// because adding two absolute temperatures is physically meaningless
    /// (e.g. 100°C + 100°C ≠ 200°C in any real-world sense).
    /// </summary>
    public class TemperatureMeasurable : IMeasurable
    {
        private readonly TemperatureUnit _unit;

        public TemperatureMeasurable(TemperatureUnit unit)
        {
            _unit = unit;
        }

        // ── Conversion factor (not meaningful for temperature) ────────────────
        // Kept to satisfy the interface contract; use ConvertToBaseUnit instead.

        public double GetConversionFactor()
        {
            // Temperature has no single multiplicative factor.
            // Returning 1.0 as a safe default; direct use is discouraged.
            return 1.0;
        }

        // ── Core conversions (Celsius as base) ───────────────────────────────

        /// <summary>
        /// Converts a value in this unit to the base unit (Celsius).
        /// </summary>
        public double ConvertToBaseUnit(double value)
        {
            switch (_unit)
            {
                case TemperatureUnit.CELSIUS:
                    return value;                           // already base

                case TemperatureUnit.FAHRENHEIT:
                    return (value - 32.0) * 5.0 / 9.0;    // °F → °C

                default:
                    throw new ArgumentException(
                        $"Unknown TemperatureUnit: {_unit}");
            }
        }

        /// <summary>
        /// Converts a value in the base unit (Celsius) to this unit.
        /// </summary>
        public double ConvertFromBaseUnit(double baseValue)
        {
            switch (_unit)
            {
                case TemperatureUnit.CELSIUS:
                    return baseValue;                        // already target

                case TemperatureUnit.FAHRENHEIT:
                    return baseValue * 9.0 / 5.0 + 32.0;   // °C → °F

                default:
                    throw new ArgumentException(
                        $"Unknown TemperatureUnit: {_unit}");
            }
        }

        // ── IMeasurable ───────────────────────────────────────────────────────

        public string GetUnitName()        => _unit.ToString();
        public string GetMeasurementType() => "TEMPERATURE";
        public bool   SupportsArithmetic() => false;

        /// <summary>
        /// Temperature does not support ADD, SUBTRACT, or DIVIDE.
        /// Throws NotSupportedException for any of those operations.
        /// COMPARE and CONVERT are handled at the service level and do
        /// not call this method.
        /// </summary>
        public void ValidateOperationSupport(string operation)
        {
            switch (operation?.ToUpperInvariant())
            {
                case "ADD":
                case "SUBTRACT":
                case "DIVIDE":
                    throw new NotSupportedException(
                        $"Operation '{operation}' is not supported for " +
                        $"TEMPERATURE. Only COMPARE and CONVERT are allowed.");

                // COMPARE and CONVERT are always valid — no-op.
                default:
                    break;
            }
        }

        public override string ToString() => $"TemperatureMeasurable({_unit})";
    }
}