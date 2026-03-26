using ApiEcommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce;

// El DbContext es la unidad de trabajo que representa una sesión con la base de datos
public class ApplicationDbContext : DbContext
{
    // El constructor recibe las configuraciones (como la cadena de conexión) 
    // y las pasa a la clase base (base) de Entity Framework.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        // Aquí no solemos escribir lógica, EF se encarga de la inicialización.
    }

    // Un DbSet representa una colección de entidades en el código 
    // que se mapea directamente a una tabla física en la base de datos.
    // En este caso: La clase 'Category' se convertirá en la tabla 'Categories'.
    public DbSet<Category> Categories { get; set; }
}