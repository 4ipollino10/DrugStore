using DrugStoreAPI.Data;
using DrugStoreAPI.Repositories;
using DrugStoreAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration
        .GetConnectionString("DrugStoreConnectionString")), ServiceLifetime.Singleton);

builder.Services.AddSingleton<IMedicamentsService, MedicamentsService>();
builder.Services.AddSingleton<IMedicamentsRepository, MedicamentsRepository>();
builder.Services.AddSingleton<IOrdersService, OrdersService>();
builder.Services.AddSingleton<IOrdersRepository, OrdersRepository>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
