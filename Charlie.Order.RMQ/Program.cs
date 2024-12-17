using Charlie.Order.DataAccess;
using Charlie.Order.DataAccess.DataModels;
using Charlie.Order.DataAccess.Repositories;
using Charlie.Order.RMQ;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Register DbContext
var connectionString = builder.Configuration.GetConnectionString("OrderDb");
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add other services
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();
