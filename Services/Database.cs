using System.Data;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Options;
using TrackingApi.Config;

namespace TrackingApi.Services;

public class Database
{
    private readonly string _connectionString;
    
    public Database(IOptions<DatabaseOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
        SqlMapper.AddTypeHandler(new DateTimeHandler());
    }

    public IDbConnection GetConnection(string connectionString)
    {
        return new NpgsqlConnection(connectionString);
    }

    public IDbConnection GetConnection()
    {
        return GetConnection(_connectionString);
    }
}

public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.Value = value;
    }

    public override DateTime Parse(object value)
    {
        return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
    }
}
