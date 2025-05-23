﻿using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using HyPlayer.Web.DbContexts;
using HyPlayer.Web.Extensions;
using HyPlayer.Web.Implementations;
using HyPlayer.Web.Interfaces;
using HyPlayer.Web.Repositories;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Serilog;
using ZiggyCreatures.Caching.Fusion;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File($"{Environment.CurrentDirectory}/log/log.log", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Services.AddHttpClient();
builder.Logging.AddSerilog();
builder.Services.Configure<JsonOptions>(option =>
{
    option.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
});
builder.Services.AddDbContext<SqliteDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("SQLite") ?? "Data Source=data/database.db",
        contextOptionsBuilder =>
        {
            contextOptionsBuilder.MigrationsAssembly(typeof(SqliteDbContext).Assembly.GetName().Name ?? "");
        });
});

builder.Services.AddFusionCache().WithDefaultEntryOptions(
    option =>
    {
        option.Duration = TimeSpan.FromMinutes(1);
    }).AsHybridCache();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(SqliteRepository<,>));
builder.Services.AddTransient<IEmailService, SmtpMailService>();
builder.Services.AddSingleton<IAdminRepository, AdminConfigurationRepository>();
builder.Services.AddSingleton<IEmailTemplateProvider, FileEmailTemplateProvider>();
builder.Services.AddTransient<IUpdateBroadcaster, EmailUpdateBroadcaster>();
builder.Services.AddTransient<IUpdateBroadcaster, TelegramBroadcaster>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsByAssembly(Assembly.GetExecutingAssembly());

#if DEBUG
builder.Services.AddCors();
#endif

var app = builder.Build();

#if DEBUG
app.UseCors(policyBuilder => { policyBuilder.AllowAnyOrigin(); });
#endif

using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<SqliteDbContext>();
Directory.CreateDirectory("data");
if (context.Database.GetMigrations().Any())
    await context.Database.MigrateAsync();

await context.Database.EnsureCreatedAsync();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseEndpoints();

app.Run("http://*:5898");