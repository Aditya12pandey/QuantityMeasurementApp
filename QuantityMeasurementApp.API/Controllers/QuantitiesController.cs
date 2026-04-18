using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.API.Models;
using QuantityMeasurementAppBusiness;
using QuantityMeasurementAppEntity.DTOs;
using QuantityMeasurementAppEntity.Mappings;
using QuantityMeasurementAppRepository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace QuantityMeasurementApp.API.Controllers;

[ApiController]
[Route("api/v1/quantities")]
[Authorize]
public class QuantitiesController : ControllerBase
{
    private readonly IQuantityMeasurementService _service;
    private readonly IQuantityMeasurementRepository _repository;

    public QuantitiesController(
        IQuantityMeasurementService service,
        IQuantityMeasurementRepository repository)
    {
        _service = service;
        _repository = repository;
    }

    [HttpPost("compare")]
    [ProducesResponseType(typeof(CompareResultDto), StatusCodes.Status200OK)]
    public ActionResult<CompareResultDto> Compare([FromBody] TwoQuantityRequest request)
    {
        bool equal = _service.Compare(GetUserId(), request.First, request.Second);
        return Ok(new CompareResultDto { Equal = equal });
    }

    [HttpPost("convert")]
    [ProducesResponseType(typeof(QuantityDTO), StatusCodes.Status200OK)]
    public ActionResult<QuantityDTO> Convert([FromBody] ConvertRequest request)
    {
        QuantityDTO result = _service.Convert(GetUserId(), request.Quantity, request.TargetUnit);
        return Ok(result);
    }

    [HttpPost("add")]
    [ProducesResponseType(typeof(QuantityDTO), StatusCodes.Status200OK)]
    public ActionResult<QuantityDTO> Add([FromBody] AddSubtractRequest request)
    {
        QuantityDTO result = _service.Add(GetUserId(), request.First, request.Second, request.TargetUnit);
        return Ok(result);
    }

    [HttpPost("subtract")]
    [ProducesResponseType(typeof(QuantityDTO), StatusCodes.Status200OK)]
    public ActionResult<QuantityDTO> Subtract([FromBody] AddSubtractRequest request)
    {
        QuantityDTO result = _service.Subtract(GetUserId(), request.First, request.Second, request.TargetUnit);
        return Ok(result);
    }

    [HttpPost("divide")]
    [ProducesResponseType(typeof(DivideResultDto), StatusCodes.Status200OK)]
    public ActionResult<DivideResultDto> Divide([FromBody] TwoQuantityRequest request)
    {
        double value = _service.Divide(GetUserId(), request.First, request.Second);
        return Ok(new DivideResultDto { Value = value });
    }

    [HttpGet("history")]
    [ProducesResponseType(typeof(IReadOnlyList<QuantityMeasurementRecordDto>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<QuantityMeasurementRecordDto>> GetHistory()
    {
        return Ok(_repository.GetAllMeasurements(GetUserId()).ToRecordDtoList());
    }

    [HttpGet("history/operation/{operationType}")]
    [ProducesResponseType(typeof(IReadOnlyList<QuantityMeasurementRecordDto>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<QuantityMeasurementRecordDto>> GetHistoryByOperation(
        [FromRoute] string operationType)
    {
        return Ok(_repository.GetByOperationType(GetUserId(), operationType).ToRecordDtoList());
    }

    [HttpGet("history/measurement/{measurementType}")]
    [ProducesResponseType(typeof(IReadOnlyList<QuantityMeasurementRecordDto>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<QuantityMeasurementRecordDto>> GetHistoryByMeasurement(
        [FromRoute] string measurementType)
    {
        return Ok(_repository.GetByMeasurementType(GetUserId(), measurementType).ToRecordDtoList());
    }

    [HttpGet("count")]
    [ProducesResponseType(typeof(MeasurementCountDto), StatusCodes.Status200OK)]
    public ActionResult<MeasurementCountDto> Count()
    {
        return Ok(new MeasurementCountDto { Count = _repository.GetCount(GetUserId()) });
    }

    private string GetUserId()
    {
        // Extract UserId from JWT NameIdentifier claim
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
               ?? throw new UnauthorizedAccessException("User not identified.");
    }
}
