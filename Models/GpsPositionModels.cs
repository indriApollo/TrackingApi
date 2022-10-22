namespace TrackingApi.Models;

public class GpsPositionDtoOut
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public short Altitude { get; set; }
    public short Heading { get; set; }
    public short Speed { get; set; }
    public DateTime Timestamp { get; set; }
}

public class GpsPositionDbOut
{
    public int Latitude { get; set; }
    public int Longitude { get; set; }
    public short Altitude { get; set; }
    public short Heading { get; set; }
    public short Speed { get; set; }
    public DateTime TimestampUtc { get; set; }
}

public static class GpsPositionExtensions
{
    public static GpsPositionDtoOut AsDtoOut(this GpsPositionDbOut gpsPositionDbOut)
    {
        return new()
        {
            Latitude = gpsPositionDbOut.Latitude / 10000000d,
            Longitude = gpsPositionDbOut.Longitude / 10000000d,
            Altitude = gpsPositionDbOut.Altitude,
            Heading = gpsPositionDbOut.Heading,
            Speed = gpsPositionDbOut.Speed,
            Timestamp = gpsPositionDbOut.TimestampUtc
        };
    }
}
