using Microsoft.AspNetCore.Mvc;
using TrackingApi.Models;

namespace TrackingApi.Controllers;

[ApiController]
[Route("api/v1/devices/{devId}/positions")]
public class GpsPositionController : ControllerBase
{
    private readonly ILogger<GpsPositionController> _logger;
    private readonly TelematicsDataRepository _dataRepository;

    public GpsPositionController(ILogger<GpsPositionController> logger, TelematicsDataRepository dataRepository)
    {
        _logger = logger;
        _dataRepository = dataRepository;
    }

    [HttpGet]
    public async Task<ActionResult> GetGpsPositions([FromRoute] int devId, [FromQuery] TimeRangeRequestModel timeRange)
    {
        var dbPos = await _dataRepository.GetGpsPositions(devId, timeRange.From?.UtcDateTime, timeRange.To?.UtcDateTime, 1000, 0);
        var dtoPos = dbPos.Select(p => p.AsDtoOut());

        return Ok(new DataResponse<IEnumerable<GpsPositionDtoOut>> { Data = dtoPos });
    }

    [Route("latest")]
    [HttpGet]
    public async Task<ActionResult> GetLatestGpsPosition([FromRoute] int devId)
    {
        var dbPos = (await _dataRepository.GetGpsPositions(devId, null, null, 1, 0)).First();
        var dtoPos =  dbPos.AsDtoOut();

        return Ok(new DataResponse<GpsPositionDtoOut> { Data = dtoPos });
    }
}
