namespace ApiEcommerce.Models.DTOs;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;


    public decimal Price { get; set; }

    public string? ImageUrl { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty; //PROD-0001-BLK-M

    public int Stock { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;

    //Relacion con Cateegory
    public int CategoryId { get; set; }


    public Category? Category { get; set; }
}
