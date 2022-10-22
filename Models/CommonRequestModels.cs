namespace TrackingApi.Models;

public class TimeRangeRequestModel
{
    public DateTimeOffset? From { get; set; }
    public DateTimeOffset? To { get; set; }
}

public class DataResponse<T>
{
    public T Data { get; set; } = default(T)!;
}
