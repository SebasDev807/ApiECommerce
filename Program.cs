using ApiEcommerce;
using ApiEcommerce.Constants;
using ApiEcommerce.Repositories;
using ApiEcommerce.Repositories.Interfaces;
using ApiEcommerce.Utils;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

string? dbConnectionString = builder.Configuration.GetConnectionString("ConexionSql");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConnectionString));

// Agregar repositorios al contenedor de servicios
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    string? secretKey = configuration.GetValue<string>("ApiSettings:SecretKey")!;
    return new TokenService(secretKey);
});

// Agregar AutoMapper para mapear entre entidades y DTOs
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Agregar servicios para controladores y OpenAPI
builder.Services.AddControllers();

// Agregar OpenAPI para documentación de la API
builder.Services.AddOpenApi();

// Configurar CORS para permitir solicitudes desde el frontend
builder.Services.AddCors(options =>
    {
        options.AddPolicy(PolicyNames.AllowSpeciefedOrigin,
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") 
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        }
        );
    }
);



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Redirigir HTTP a HTTPS
app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors(PolicyNames.AllowSpeciefedOrigin);

// Habilitar autenticación y autorización
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

// Ejecutar la aplicación
app.Run();