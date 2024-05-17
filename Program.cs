#region Services
using Data;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddSingleton<PasswordHasher>();

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(Configuration.Configurations.ConnectionStrings.Postgres));
#endregion

#region Middleware
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
