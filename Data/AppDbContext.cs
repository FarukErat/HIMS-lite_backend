/*
dotnet ef migrations add InitialMigration
dotnet ef database update
dotnet ef migrations remove
*/

using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
