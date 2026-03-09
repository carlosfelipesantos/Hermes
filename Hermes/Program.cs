using Hermes.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<HermesBD>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); //conexao com Banco de Dados

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());//registrando automapper no container de injeção de dependência para mapear objetos de forma automatica

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
