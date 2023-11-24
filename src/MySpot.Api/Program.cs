using MySpot.Application;
using MySpot.Core;
using MySpot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCore()
    .AddAplication()
    .AddInfrastructure()
    .AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
