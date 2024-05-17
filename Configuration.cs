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
    public readonly string Postgres = "Host=localhost;Port=5432;Database=HimsLite;Username=postgres;Password=postgres;Pooling=true;";
    public readonly string Redis = "redis://localhost:6379";
}
