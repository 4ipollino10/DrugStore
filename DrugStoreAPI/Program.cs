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

builder.Services.AddScoped<IMedicamentsService, MedicamentsService>();
builder.Services.AddScoped<IMedicamentsRepository, MedicamentsRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseRouting();

app.MapControllers();

app.Run();
