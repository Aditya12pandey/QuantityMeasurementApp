using System;
using QuantityMeasurementAppBusiness.Exceptions;
using QuantityMeasurementAppBusiness.Implementations;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppEntity.DTOs;
using QuantityMeasurementAppEntity.Entity;
using QuantityMeasurementAppEntity.Enums;
using QuantityMeasurementAppRepository.Interfaces;

namespace QuantityMeasurementAppBusiness
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementServiceImpl(
            IQuantityMeasurementRepository repository)
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));
        }

        // ── Compare ───────────────────────────────────────────────────────

        public bool Compare(string userId, QuantityDTO dto1, QuantityDTO dto2)
        {
            try
            {
                ValidateNotNull(dto1, "quantity1");
                ValidateNotNull(dto2, "quantity2");
                ValidateSameCategory(dto1, dto2, "COMPARE");

                IMeasurable m1 = ToUnit(dto1);
                IMeasurable m2 = ToUnit(dto2);

                double base1 = m1.ConvertToBaseUnit(dto1.Value);
                double base2 = m2.ConvertToBaseUnit(dto2.Value);
                bool result  = Math.Abs(base1 - base2) < 0.0001;

                var entity = new QuantityEntity("COMPARE", dto1, dto2, result.ToString(), null, null);
                entity.UserId = userId;
                _repository.Save(entity);

                return result;
            }
            catch (QuantityMeasurementException)
            {
                try { 
                    var entity = new QuantityEntity("COMPARE", dto1, dto2, "Comparison failed", true);
                    entity.UserId = userId;
                    _repository.Save(entity); 
                }
                catch { }
                throw;
            }
            catch (Exception ex)
            {
                try { 
                    var entity = new QuantityEntity("COMPARE", dto1, dto2, ex.Message, true);
                    entity.UserId = userId;
                    _repository.Save(entity); 
                }
                catch { }
                throw new QuantityMeasurementException("Comparison failed: " + ex.Message, ex);
            }
        }

        // ── Convert ───────────────────────────────────────────────────────

        public QuantityDTO Convert(string userId, QuantityDTO quantity, QuantityDTO targetUnitDto)
        {
            try
            {
                ValidateNotNull(quantity,      "quantity");
                ValidateNotNull(targetUnitDto, "targetUnit");
                ValidateSameCategory(quantity, targetUnitDto, "CONVERT");

                IMeasurable source = ToUnit(quantity);
                IMeasurable target = ToUnit(targetUnitDto);

                double baseValue   = source.ConvertToBaseUnit(quantity.Value);
                double resultValue = Math.Round(
                    target.ConvertFromBaseUnit(baseValue), 2);

                QuantityDTO result = new QuantityDTO(
                    resultValue, target.GetUnitName(),
                    target.GetMeasurementType());

                var entity = new QuantityEntity("CONVERT", quantity, result);
                entity.UserId = userId;
                _repository.Save(entity);
                return result;
            }
            catch (QuantityMeasurementException) { throw; }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException(
                    "Conversion failed: " + ex.Message, ex);
            }
        }

        // ── Add ───────────────────────────────────────────────────────────

        public QuantityDTO Add(string userId, QuantityDTO dto1, QuantityDTO dto2,
                               QuantityDTO targetUnitDto)
        {
            try
            {
                ValidateNotNull(dto1,          "quantity1");
                ValidateNotNull(dto2,          "quantity2");
                ValidateNotNull(targetUnitDto, "targetUnit");
                ValidateSameCategory(dto1, dto2, "ADD");

                IMeasurable m1     = ToUnit(dto1);
                IMeasurable m2     = ToUnit(dto2);
                IMeasurable target = ToUnit(targetUnitDto);

                m1.ValidateOperationSupport("ADD");

                double baseResult  = m1.ConvertToBaseUnit(dto1.Value)
                                   + m2.ConvertToBaseUnit(dto2.Value);
                double resultValue = Math.Round(
                    target.ConvertFromBaseUnit(baseResult), 2);

                QuantityDTO result = new QuantityDTO(
                    resultValue, target.GetUnitName(),
                    target.GetMeasurementType());

                var entity = new QuantityEntity("ADD", dto1, dto2, resultValue.ToString("F2"), target.GetUnitName(), target.GetMeasurementType());
                entity.UserId = userId;
                _repository.Save(entity);

                return result;
            }
            catch (NotSupportedException)        { throw; }
            catch (QuantityMeasurementException) { throw; }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException(
                    "Addition failed: " + ex.Message, ex);
            }
        }

        // ── Subtract ──────────────────────────────────────────────────────

        public QuantityDTO Subtract(string userId, QuantityDTO dto1, QuantityDTO dto2,
                                    QuantityDTO targetUnitDto)
        {
            try
            {
                ValidateNotNull(dto1,          "quantity1");
                ValidateNotNull(dto2,          "quantity2");
                ValidateNotNull(targetUnitDto, "targetUnit");
                ValidateSameCategory(dto1, dto2, "SUBTRACT");

                IMeasurable m1     = ToUnit(dto1);
                IMeasurable m2     = ToUnit(dto2);
                IMeasurable target = ToUnit(targetUnitDto);

                m1.ValidateOperationSupport("SUBTRACT");

                double baseResult  = m1.ConvertToBaseUnit(dto1.Value)
                                   - m2.ConvertToBaseUnit(dto2.Value);
                double resultValue = Math.Round(
                    target.ConvertFromBaseUnit(baseResult), 2);

                QuantityDTO result = new QuantityDTO(
                    resultValue, target.GetUnitName(),
                    target.GetMeasurementType());

                var entity = new QuantityEntity("SUBTRACT", dto1, dto2, resultValue.ToString("F2"), target.GetUnitName(), target.GetMeasurementType());
                entity.UserId = userId;
                _repository.Save(entity);

                return result;
            }
            catch (NotSupportedException)        { throw; }
            catch (QuantityMeasurementException) { throw; }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException(
                    "Subtraction failed: " + ex.Message, ex);
            }
        }

        // ── Divide ────────────────────────────────────────────────────────

        public double Divide(string userId, QuantityDTO dto1, QuantityDTO dto2)
        {
            try
            {
                ValidateNotNull(dto1, "quantity1");
                ValidateNotNull(dto2, "quantity2");
                ValidateSameCategory(dto1, dto2, "DIVIDE");

                IMeasurable m1 = ToUnit(dto1);
                IMeasurable m2 = ToUnit(dto2);

                m1.ValidateOperationSupport("DIVIDE");

                double base2 = m2.ConvertToBaseUnit(dto2.Value);
                if (Math.Abs(base2) < 1e-10)
                    throw new ArithmeticException(
                        "Division by zero is not allowed");

                double result = m1.ConvertToBaseUnit(dto1.Value) / base2;

                var entity = new QuantityEntity("DIVIDE", dto1, dto2, result.ToString("F6"), null, null);
                entity.UserId = userId;
                _repository.Save(entity);

                return result;
            }
            catch (NotSupportedException)        { throw; }
            catch (QuantityMeasurementException) { throw; }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException(
                    "Division failed: " + ex.Message, ex);
            }
        }

        // ── Private helpers ───────────────────────────────────────────────

        private IMeasurable ToUnit(QuantityDTO dto)
        {
            switch (dto.MeasurementType?.ToUpperInvariant())
            {
                case "LENGTH":
                    if (Enum.TryParse<LengthUnit>(dto.UnitName, true, out var lu))
                        return new LengthMeasurable(lu);
                    throw new QuantityMeasurementException(
                        $"Unknown LengthUnit: {dto.UnitName}");
                case "WEIGHT":
                    if (Enum.TryParse<WeightUnit>(dto.UnitName, true, out var wu))
                        return new WeightMeasurable(wu);
                    throw new QuantityMeasurementException(
                        $"Unknown WeightUnit: {dto.UnitName}");
                case "VOLUME":
                    if (Enum.TryParse<VolumeUnit>(dto.UnitName, true, out var vu))
                        return new VolumeMeasurable(vu);
                    throw new QuantityMeasurementException(
                        $"Unknown VolumeUnit: {dto.UnitName}");
                case "TEMPERATURE":
                    if (Enum.TryParse<TemperatureUnit>(dto.UnitName, true, out var tu))
                        return new TemperatureMeasurable(tu);
                    throw new QuantityMeasurementException(
                        $"Unknown TemperatureUnit: {dto.UnitName}");
                default:
                    throw new QuantityMeasurementException(
                        $"Unknown measurement type: {dto.MeasurementType}");
            }
        }

        private void ValidateNotNull(QuantityDTO dto, string name)
        {
            if (dto == null)
                throw new QuantityMeasurementException(
                    $"Operand '{name}' cannot be null");
        }

        private void ValidateSameCategory(QuantityDTO dto1, QuantityDTO dto2,
                                          string operation)
        {
            if (!string.Equals(dto1.MeasurementType, dto2.MeasurementType,
                    StringComparison.OrdinalIgnoreCase))
                throw new QuantityMeasurementException(
                    $"Cannot perform {operation} across different categories: " +
                    $"{dto1.MeasurementType} vs {dto2.MeasurementType}");
        }
    }
}