// Create Serilog

using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using HyPlayer.Web.DbContexts;
using HyPlayer.Web.Extensions;
using HyPlayer.Web.Infrastructure;
using HyPlayer.Web.Repositories;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File($"{Environment.CurrentDirectory}/log/log.log", rollingInterval: RollingInterval.Day)
#if DEBUG
    .WriteTo.Console()
#endif
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Services.Configure<JsonOptions>(option =>
{
    option.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
});
builder.Services.AddDbContext<SqliteDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("SQLite") ?? "Data Source=database.db",
        contextOptionsBuilder =>
        {
            contextOptionsBuilder.MigrationsAssembly(typeof(SqliteDbContext).Assembly.GetName().Name ?? "");
        });
});
builder.Services.AddScoped(typeof(IRepository<,>), typeof(SqliteRepository<,>));
builder.Services.AddFluentValidation();
builder.Services.AddEndpointsByAssembly(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseStaticFiles();
app.UseEndpoints();

app.Run();