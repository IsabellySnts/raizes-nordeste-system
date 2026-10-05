using FluentValidation;
using MediatR;
using Microsoft.OpenApi;
using RaizesDoNordeste.Application;
using RaizesDoNordeste.Application.Commons.Behaviors;
using RaizesDoNordeste.Infrastructure;

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

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Rede Raízes do Nordeste API",
        Version = "v1",
        Description = "API para gerenciamento da rede de franquias Raízes do Nordeste — pedidos, cardápio, estoque, pagamentos (Stripe), programa de fidelidade e conformidade LGPD.",
        Contact = new OpenApiContact
        {
            Name = "Isabelly dos Santos",
            Email = "isabellydossantos112@gmail.com"
        }
    });

    options.EnableAnnotations();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});



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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Development");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();