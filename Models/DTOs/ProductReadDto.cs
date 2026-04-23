namespace ApiEcommerce.Models.DTOs;

public class ProductReadDto
{
    public ProductReadDto(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        CategoryName = product.Category?.Name ?? "Sin Categoría";
    }
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    //Relacion con Cateegory
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

}