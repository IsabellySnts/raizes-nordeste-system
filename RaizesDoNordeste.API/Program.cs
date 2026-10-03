using FluentValidation;
using MediatR;
using RaizesDoNordeste.Application;
using RaizesDoNordeste.Application.Commons.Behaviors;
using RaizesDoNordeste.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(RaizesDoNordeste.Application.Commons.ResultViewModel<>).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(
    typeof(RaizesDoNordeste.Application.Commons.ResultViewModel<>).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("Development");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();