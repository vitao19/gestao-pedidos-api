using FluentValidation;
using GestaoPedidos.Api.Application.Interfaces;
using GestaoPedidos.Api.Application.Services;
using GestaoPedidos.Api.Application.Validators;
using GestaoPedidos.Api.Domain.Interfaces;
using GestaoPedidos.Api.Infrastructure.Data;
using GestaoPedidos.Api.Infrastructure.Repositories;
using GestaoPedidos.Api.Presentation.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<TratadorExcecoesGlobal>();

// Banco de Dados em Memória
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("GestaoPedidosDb"));

// Repositórios e Unit of Work 
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Serviços 
builder.Services.AddScoped<IPedidoService, PedidoService>();

// FluentValidation 
builder.Services.AddValidatorsFromAssemblyContaining<CriarPedidoDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CriarItemPedidoDtoValidator>();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(_ => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
