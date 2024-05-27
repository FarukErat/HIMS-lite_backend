namespace Configuration;

public static class Configurations
{
    public static readonly ConnectionStrings ConnectionStrings = new();
    public static readonly TimeSpan SessionExpiry = TimeSpan.FromDays(7);
    public const string SessionIdCookieKey = "SID";
    public const string SessionItemsKey = "Session";
}

public sealed class ConnectionStrings
{
    public readonly string Postgres;
    public readonly string Redis;

    public ConnectionStrings()
    {
        Postgres = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
            ?? throw new ArgumentNullException("POSTGRES_CONNECTION_STRING");
        Redis = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
            ?? throw new ArgumentNullException("REDIS_CONNECTION_STRING");
    }
}
