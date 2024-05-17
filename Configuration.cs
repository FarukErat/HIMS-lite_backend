namespace Configuration;

public static class Configurations
{
    public static readonly ConnectionStrings ConnectionStrings = new();
}

public sealed class ConnectionStrings
{
    public readonly string Postgres= "Host=localhost;Port=5432;Database=HimsLite;Username=postgres;Password=root;Pooling=true;";
}
