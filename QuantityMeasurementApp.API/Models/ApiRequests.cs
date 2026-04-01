using System.ComponentModel.DataAnnotations;
using QuantityMeasurementAppEntity.DTOs;

namespace QuantityMeasurementApp.API.Models;

public class TwoQuantityRequest
{
    [Required]
    public QuantityDTO First { get; set; } = null!;

    [Required]
    public QuantityDTO Second { get; set; } = null!;
}

public class ConvertRequest
{
    [Required]
    public QuantityDTO Quantity { get; set; } = null!;

    [Required]
    public QuantityDTO TargetUnit { get; set; } = null!;
}

public class AddSubtractRequest : TwoQuantityRequest
{
    [Required]
    public QuantityDTO TargetUnit { get; set; } = null!;
}
