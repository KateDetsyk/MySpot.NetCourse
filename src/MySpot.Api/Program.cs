using MySpot.Application;
using MySpot.Core;
using MySpot.Core.Exceptions;
using MySpot.Infrastructure;
using MySpot.Infrastructure.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCore()
    .AddAplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseInfrastructure();

app.Run();
