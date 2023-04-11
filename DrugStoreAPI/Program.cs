using DrugStoreAPI.Configuration;
using DrugStoreAPI.Data;
using DrugStoreAPI.Repositories;
using DrugStoreAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => 
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies().UseNpgsql(
        builder.Configuration
        .GetConnectionString("DrugStoreConnectionString")), ServiceLifetime.Singleton);

builder.Services.AddSingleton<IMedicamentsService, MedicamentsService>();
builder.Services.AddSingleton<IMedicamentsRepository, MedicamentsRepository>();
builder.Services.AddSingleton<IOrdersService, OrdersService>();
builder.Services.AddSingleton<IOrdersRepository, OrdersRepository>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
