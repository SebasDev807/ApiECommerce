// Punto de entrada principal: Inicializa el constructor de la aplicación web
var builder = WebApplication.CreateBuilder(args);

// --- SECCIÓN: Registro de Servicios (Inyección de Dependencias) ---

// Registra el soporte para controladores de API
builder.Services.AddControllers(); 

// Configura la generación de documentación técnica con OpenAPI (Swagger)
builder.Services.AddOpenApi();

// Construye la instancia de la aplicación (WebApplication)
var app = builder.Build();

// --- SECCIÓN: Configuración del Pipeline de Middlewares (HTTP Request) ---

// Habilita el entorno de documentación interactiva solo en Desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware para forzar la redirección automática de tráfico HTTP a HTTPS
app.UseHttpsRedirection();

// Middleware de Autorización: Gestiona el acceso basado en políticas y roles
app.UseAuthorization();

// Mapeo dinámico: Escanea y expone los Endpoints definidos en los Controladores
app.MapControllers();

// Ejecuta la aplicación e inicia la escucha de peticiones
app.Run();