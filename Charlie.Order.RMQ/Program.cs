using Charlie.Order.DataAccess;
using Charlie.Order.DataAccess.DataModels;
using Charlie.Order.DataAccess.Repositories;
using Charlie.Order.RMQ;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OrderDb");

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSingleton<RabbitMqClient>();

builder.Services.AddScoped<IOrderRepository<OrderModel>, OrderRepository>();
//builder.Services.AddSingleton<CustomerMapper>();

builder.Services.AddHostedService<Worker>();



var host = builder.Build();
await host.RunAsync();
