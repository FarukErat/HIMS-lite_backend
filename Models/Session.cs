using Enums;
using Redis.OM.Modeling;

namespace Models;

[Document(StorageType = StorageType.Json, IndexName = "HimsSessions", Prefixes = ["HimsSession"])]
public sealed record class Session
{
    [RedisIdField]
    public Guid Id { get; set; }

    [Indexed]
    public Guid UserId { get; set; }

    [Indexed]
    public string? Email { get; set; }

    [Indexed]
    public List<Role> Roles { get; set; } = [];

    [Indexed]
    public string? IpAddress { get; set; }

    [Indexed]
    public string? UserAgent { get; set; }

    [Indexed]
    public DateTime CreatedAt { get; set; }

    [Indexed]
    public DateTime ExpireAt { get; set; }
};
