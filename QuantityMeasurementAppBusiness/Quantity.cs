    using System;
    using QuantityMeasurementAppBusiness.Implementations;
    using QuantityMeasurementAppBusiness.Interfaces;
    using QuantityMeasurementAppEntity.Enums;

    namespace QuantityMeasurementAppBusiness
    {
        public class Quantity<U> where U : IMeasurable
        {
            private readonly double _value;
            private readonly U _unit;

            public Quantity(double value, U unit)
            {
                _value = value;
                _unit  = unit;
            }

            public double Value => _value;
            public U Unit       => _unit;

            // Resolve IMeasurable adapter for the current unit
            private IMeasurable GetMeasurable()
            {
                if (_unit is LengthUnit lu)      return new LengthMeasurable(lu);
                if (_unit is WeightUnit wu)      return new WeightMeasurable(wu);
                if (_unit is VolumeUnit vu)      return new VolumeMeasurable(vu);
                if (_unit is TemperatureUnit tu) return new TemperatureMeasurable(tu);
                throw new ArgumentException($"Unsupported unit type: {typeof(U).Name}");
            }

            // Resolve IMeasurable adapter for a given target unit
            private IMeasurable GetMeasurable(U targetUnit)
            {
                if (targetUnit is LengthUnit lu)      return new LengthMeasurable(lu);
                if (targetUnit is WeightUnit wu)      return new WeightMeasurable(wu);
                if (targetUnit is VolumeUnit vu)      return new VolumeMeasurable(vu);
                if (targetUnit is TemperatureUnit tu) return new TemperatureMeasurable(tu);
                throw new ArgumentException($"Unsupported unit type: {typeof(U).Name}");
            }

            // Convert this quantity's value to the base unit
            public double ToBase() => GetMeasurable().ConvertToBaseUnit(_value);

            // Validate arithmetic support for the current unit type
            private void ValidateArithmetic(string operation)
                => GetMeasurable().ValidateOperationSupport(operation);

            // Validate operand is not null and both values are finite
            private void ValidateOperand(Quantity<U> other)
            {
                if (other == null)
                    throw new ArgumentException("Operand cannot be null");
                if (!double.IsFinite(_value))
                    throw new ArgumentException("This quantity value must be finite");
                if (!double.IsFinite(other._value))
                    throw new ArgumentException("Operand value must be finite");
            }

            private static double Round(double value) => Math.Round(value, 2);

            // Convert to a target unit
            public Quantity<U> ConvertTo(U targetUnit)
            {
                double result = GetMeasurable(targetUnit).ConvertFromBaseUnit(ToBase());
                return new Quantity<U>(result, targetUnit);
            }

            // Add — result in this unit
            public Quantity<U> Add(Quantity<U> other) => Add(other, _unit);

            // Add — result in specified target unit
            public Quantity<U> Add(Quantity<U> other, U targetUnit)
            {
                ValidateArithmetic("ADD");
                ValidateOperand(other);
                double baseResult = ToBase() + other.ToBase();
                double result = GetMeasurable(targetUnit).ConvertFromBaseUnit(baseResult);
                return new Quantity<U>(Round(result), targetUnit);
            }

            // Subtract — result in this unit
            public Quantity<U> Subtract(Quantity<U> other) => Subtract(other, _unit);

            // Subtract — result in specified target unit
            public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
            {
                ValidateArithmetic("SUBTRACT");
                ValidateOperand(other);
                double baseResult = ToBase() - other.ToBase();
                double result = GetMeasurable(targetUnit).ConvertFromBaseUnit(baseResult);
                return new Quantity<U>(Round(result), targetUnit);
            }

            // Divide — returns dimensionless ratio
            public double Divide(Quantity<U> other)
            {
                ValidateArithmetic("DIVIDE");
                ValidateOperand(other);
                double base2 = other.ToBase();
                if (Math.Abs(base2) < 1e-10)
                    throw new ArithmeticException("Division by zero is not allowed");
                return ToBase() / base2;
            }

            // Equality based on base-unit comparison
            public override bool Equals(object obj)
            {
                if (obj is not Quantity<U> other) return false;
                return Math.Abs(ToBase() - other.ToBase()) < 0.0001;
            }

            public override int GetHashCode() => ToBase().GetHashCode();

            public override string ToString() => $"{_value} {_unit}";
        }
    }