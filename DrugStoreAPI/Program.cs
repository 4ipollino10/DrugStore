using DrugStoreAPI.src.Configuration;
using DrugStoreAPI.src.Repositories;
using DrugStoreAPI.src.Services;
using DrugStoreAPI.Repositories;
using DrugStoreAPI.Services;
using DrugStoreAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => 
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies().UseNpgsql(
        builder.Configuration
        .GetConnectionString("DrugStoreConnectionString")), ServiceLifetime.Scoped);

var frontAllowSpecificOrigin = "_frontAllowSpecificOrigin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: frontAllowSpecificOrigin, policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IComponentsService, ComponentsService>();
builder.Services.AddScoped<IComponentsRepository, ComponentsRepository>();
builder.Services.AddScoped<IDrugsService, DrugsService>();
builder.Services.AddScoped<IDrugsRepository, DrugsRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IClientService, ClientsService>();
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();


var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseRouting();

app.MapControllers();

app.UseCors(frontAllowSpecificOrigin);

app.Run();
