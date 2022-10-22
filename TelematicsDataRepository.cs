using TrackingApi.Services;
using TrackingApi.Models;
using TrackingApi.Config;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace TrackingApi;

public class TelematicsDataRepository
{
    private readonly Database _db;
    private readonly MemoryCache _cache;

    public TelematicsDataRepository(Database db, IOptions<DatabaseOptions> options)
    {
        _db = db;
        _cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = options.Value.DbCacheSize });
    }

    public async Task<IEnumerable<GpsPositionDbOut>> GetGpsPositions(int devId, DateTime? from, DateTime? to, int limit, int offset)
    {
        string condition = "";
        string query = $@"
        SELECT
            gps_latitude AS Latitude,
            gps_longitude AS Longitude,
            gps_altitude AS Altitude,
            gps_heading AS Heading,
            gps_speed AS Speed,
            gps_timestamp_utc AS TimestampUtc
        FROM telematics.gps_position
        WHERE gps_dev_id = @devId {condition}
        ORDER BY gps_timestamp_utc DESC
        LIMIT @limit OFFSET @offset";

        var param = new DynamicParameters(new { devId, limit, offset });

        if (from.HasValue)
        {
            condition += "AND gps_datetime_utc >= @from";
            param.Add("from", from);
        }

        if (to.HasValue)
        {
            condition += "AND gps_datetime_utc <= @to";
            param.Add("to", to);
        }

        using (var conn = _db.GetConnection())
        {
            return await conn.QueryAsync<GpsPositionDbOut>(query, param);
        }
    }
}
