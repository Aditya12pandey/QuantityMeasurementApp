using System;
using QuantityService.Models;
using QuantityService.Data;

namespace QuantityService.Business;

public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
{
    private readonly IQuantityMeasurementRepository _repository;

    public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
    {
        _repository = repository;
    }

    public bool Compare(QuantityDTO dto1, QuantityDTO dto2)
    {
        ValidateSameCategory(dto1, dto2, "COMPARE");
        IMeasurable m1 = ToUnit(dto1);
        IMeasurable m2 = ToUnit(dto2);

        double base1 = m1.ConvertToBaseUnit(dto1.Value);
        double base2 = m2.ConvertToBaseUnit(dto2.Value);
        bool result = Math.Abs(base1 - base2) < 0.0001;

        _repository.Save(new QuantityEntity("COMPARE", dto1, dto2, result.ToString()));
        return result;
    }

    public QuantityDTO Convert(QuantityDTO quantity, string targetUnit)
    {
        IMeasurable source = ToUnit(quantity);
        IMeasurable target = ToUnit(new QuantityDTO { MeasurementType = quantity.MeasurementType, UnitName = targetUnit });

        double baseValue = source.ConvertToBaseUnit(quantity.Value);
        double resultValue = Math.Round(target.ConvertFromBaseUnit(baseValue), 4);

        QuantityDTO result = new QuantityDTO
        {
            Value = resultValue,
            UnitName = target.GetUnitName(),
            MeasurementType = target.GetMeasurementType()
        };

        _repository.Save(new QuantityEntity("CONVERT", quantity, result));
        return result;
    }

    public QuantityDTO Add(QuantityDTO dto1, QuantityDTO dto2, string targetUnit)
    {
        ValidateSameCategory(dto1, dto2, "ADD");
        IMeasurable m1 = ToUnit(dto1);
        IMeasurable m2 = ToUnit(dto2);
        IMeasurable target = ToUnit(new QuantityDTO { MeasurementType = dto1.MeasurementType, UnitName = targetUnit });

        m1.ValidateOperationSupport("ADD");

        double baseResult = m1.ConvertToBaseUnit(dto1.Value) + m2.ConvertToBaseUnit(dto2.Value);
        double resultValue = Math.Round(target.ConvertFromBaseUnit(baseResult), 4);

        QuantityDTO result = new QuantityDTO
        {
            Value = resultValue,
            UnitName = target.GetUnitName(),
            MeasurementType = target.GetMeasurementType()
        };

        _repository.Save(new QuantityEntity("ADD", dto1, dto2, resultValue.ToString(), target.GetUnitName(), target.GetMeasurementType()));
        return result;
    }

    public QuantityDTO Subtract(QuantityDTO dto1, QuantityDTO dto2, string targetUnit)
    {
        ValidateSameCategory(dto1, dto2, "SUBTRACT");
        IMeasurable m1 = ToUnit(dto1);
        IMeasurable m2 = ToUnit(dto2);
        IMeasurable target = ToUnit(new QuantityDTO { MeasurementType = dto1.MeasurementType, UnitName = targetUnit });

        m1.ValidateOperationSupport("SUBTRACT");

        double baseResult = m1.ConvertToBaseUnit(dto1.Value) - m2.ConvertToBaseUnit(dto2.Value);
        double resultValue = Math.Round(target.ConvertFromBaseUnit(baseResult), 4);

        QuantityDTO result = new QuantityDTO
        {
            Value = resultValue,
            UnitName = target.GetUnitName(),
            MeasurementType = target.GetMeasurementType()
        };

        _repository.Save(new QuantityEntity("SUBTRACT", dto1, dto2, resultValue.ToString(), target.GetUnitName(), target.GetMeasurementType()));
        return result;
    }

    public double Divide(QuantityDTO dto1, QuantityDTO dto2)
    {
        ValidateSameCategory(dto1, dto2, "DIVIDE");
        IMeasurable m1 = ToUnit(dto1);
        IMeasurable m2 = ToUnit(dto2);

        m1.ValidateOperationSupport("DIVIDE");

        double base2 = m2.ConvertToBaseUnit(dto2.Value);
        if (Math.Abs(base2) < 1e-10) throw new ArithmeticException("Division by zero");

        double result = m1.ConvertToBaseUnit(dto1.Value) / base2;

        _repository.Save(new QuantityEntity("DIVIDE", dto1, dto2, result.ToString("F6")));
        return result;
    }

    private IMeasurable ToUnit(QuantityDTO dto)
    {
        return dto.MeasurementType?.ToUpperInvariant() switch
        {
            "LENGTH" => Enum.TryParse<LengthUnit>(dto.UnitName, true, out var lu) ? new LengthMeasurable(lu) : throw new QuantityException($"Unknown unit: {dto.UnitName}"),
            "WEIGHT" => Enum.TryParse<WeightUnit>(dto.UnitName, true, out var wu) ? new WeightMeasurable(wu) : throw new QuantityException($"Unknown unit: {dto.UnitName}"),
            "VOLUME" => Enum.TryParse<VolumeUnit>(dto.UnitName, true, out var vu) ? new VolumeMeasurable(vu) : throw new QuantityException($"Unknown unit: {dto.UnitName}"),
            "TEMPERATURE" => Enum.TryParse<TemperatureUnit>(dto.UnitName, true, out var tu) ? new TemperatureMeasurable(tu) : throw new QuantityException($"Unknown unit: {dto.UnitName}"),
            _ => throw new QuantityException($"Unknown measurement type: {dto.MeasurementType}")
        };
    }

    private void ValidateSameCategory(QuantityDTO dto1, QuantityDTO dto2, string op)
    {
        if (!string.Equals(dto1.MeasurementType, dto2.MeasurementType, StringComparison.OrdinalIgnoreCase))
            throw new QuantityException($"Cannot perform {op} across {dto1.MeasurementType} and {dto2.MeasurementType}");
    }
}
