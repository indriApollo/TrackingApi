using Microsoft.AspNetCore.Mvc;
using TrackingApi.Models;

namespace TrackingApi.Controllers;

[ApiController]
public class GpsPositionController : ControllerBase
{
    private readonly ILogger<GpsPositionController> _logger;
    private readonly TelematicsDataRepository _dataRepository;

    public GpsPositionController(ILogger<GpsPositionController> logger, TelematicsDataRepository dataRepository)
    {
        _logger = logger;
        _dataRepository = dataRepository;
    }

    [Route("api/v1/devices/{devId}/positions")]
    [HttpGet]
    public async Task<ActionResult> GetGpsPositions([FromRoute] int devId, [FromQuery] TimeRangeRequestModel timeRange)
    {
        var dbPos = await _dataRepository.GetGpsPositions(devId, timeRange.From?.UtcDateTime, timeRange.To?.UtcDateTime, 1000, 0);
        var dtoPos = dbPos.Select(p => new GpsPositionDtoOut
        {
            Latitude = p.Latitude / 10000000d,
            Longitude = p.Longitude / 10000000d,
            Altitude = p.Altitude,
            Heading = p.Heading,
            Speed = p.Speed,
            Timestamp = p.TimestampUtc
        });

        return Ok(new DataResponse<IEnumerable<GpsPositionDtoOut>> { Data = dtoPos });
    }
}
