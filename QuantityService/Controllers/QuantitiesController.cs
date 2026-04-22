using Microsoft.AspNetCore.Mvc;
using QuantityService.Models;
using QuantityService.Business;
using QuantityService.Data;
using Microsoft.AspNetCore.Authorization;

namespace QuantityService.Controllers;

[ApiController]
[Route("api/quantities")]
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
    public ActionResult<CompareResultDto> Compare([FromBody] TwoQuantityRequest request)
    {
        bool equal = _service.Compare(request.First, request.Second);
        return Ok(new CompareResultDto { Equal = equal });
    }

    [HttpPost("convert")]
    public ActionResult<QuantityDTO> Convert([FromBody] ConvertRequest request)
    {
        QuantityDTO result = _service.Convert(request.Quantity, request.TargetUnit);
        return Ok(result);
    }

    [HttpPost("add")]
    public ActionResult<QuantityDTO> Add([FromBody] AddSubtractRequest request)
    {
        QuantityDTO result = _service.Add(request.First, request.Second, request.TargetUnit);
        return Ok(result);
    }

    [HttpPost("subtract")]
    public ActionResult<QuantityDTO> Subtract([FromBody] AddSubtractRequest request)
    {
        QuantityDTO result = _service.Subtract(request.First, request.Second, request.TargetUnit);
        return Ok(result);
    }

    [HttpPost("divide")]
    public ActionResult<DivideResultDto> Divide([FromBody] TwoQuantityRequest request)
    {
        double value = _service.Divide(request.First, request.Second);
        return Ok(new DivideResultDto { Value = value });
    }

    [HttpGet("history")]
    public ActionResult<List<QuantityMeasurementRecordDto>> GetHistory()
    {
        var entities = _repository.GetAllMeasurements();
        var dtos = entities.Select(e => new QuantityMeasurementRecordDto
        {
            OperationId = e.OperationId,
            OperationType = e.OperationType,
            Operand1Value = e.Operand1Value,
            Operand1Unit = e.Operand1Unit,
            Operand2Value = e.Operand2Value,
            Operand2Unit = e.Operand2Unit,
            ResultValue = e.ResultValue,
            ResultUnit = e.ResultUnit,
            Timestamp = e.Timestamp
        }).ToList();
        return Ok(dtos);
    }

    [HttpGet("count")]
    public ActionResult<MeasurementCountDto> Count()
    {
        return Ok(new MeasurementCountDto { Count = _repository.GetCount() });
    }
}
