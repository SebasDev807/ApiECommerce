using ApiEcommerce;
using ApiEcommerce.Repositories;
using ApiEcommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

string? dbConnectionString = builder.Configuration.GetConnectionString("ConexionSql");

builder.Services.AddDbContext<ApplicationDbContext>(options =>  options.UseSqlServer(dbConnectionString));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);


builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();