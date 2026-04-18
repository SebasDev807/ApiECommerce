using ApiEcommerce.Models;
using ApiEcommerce.Repositories.Interfaces;

namespace ApiEcommerce.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>
    ///  Compra un producto y actualiza el stock en la base de datos
    /// </summary>
    /// <param name="name">Nombre del producto</param>
    /// <param name="quantity">Cantidad a comprar</param>
    /// <returns>True si la compra fue exitosa, false en caso contrario</returns>
    public bool BuyProduct(string name, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name) || quantity <= 0)
            return false;

        Product? product = _db.Products.FirstOrDefault(product =>
            product.Name.ToLower().Trim() == name.ToLower().Trim()
        );

        if (product == null || product.Stock < quantity)
            return false;

            product.Stock -= quantity;
        return Save();
    }

    /// <summary>
    /// Crea un producto en la base de datos
    /// </summary>
    /// <param name="product">producto a guardar</param>
    /// <returns>El metodo Save para guardar el producto</returns>
    public bool CreateProduct(Product product)
    {
        if(product == null)
            return false;
        
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        _db.Products.Add(product);
        return Save();
    }

    public bool DeleteProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public Product GetProduct(int id)
    {
        throw new NotImplementedException();
    }

    public ICollection<Product> GetProducts()
    {
        throw new NotImplementedException();
    }

    public ICollection<Product> GetProductsForCategory(int categoryId)
    {
        throw new NotImplementedException();
    }

    public bool ProductExists(int id)
    {
        throw new NotImplementedException();
    }

    public bool ProductExists(string name)
    {
        throw new NotImplementedException();
    }

    public bool Save()
    {
        throw new NotImplementedException();
    }

    public ICollection<Product> SearchProduct(string name)
    {
        throw new NotImplementedException();
    }

    public bool UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }
}
