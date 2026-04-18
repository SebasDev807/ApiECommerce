using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEcommerce.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    [Range(0,double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo. ")]

    [Column(TypeName = "decimal(18,2)")] //Ajustar precision y escala para SQL Server 
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; } = string.Empty;

    [Required]
    public string SKU { get; set; } = string.Empty; //PROD-0001-BLK-M
    
    [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un valor positivo. ")]
    public int Stock { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;

    //Relacion con Cateegory
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

}
