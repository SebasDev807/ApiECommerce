using ApiEcommerce.Models;
using ApiEcommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        // Busca el producto por nombre, ignorando mayúsculas y espacios en blanco
        Product? product = _db.Products.FirstOrDefault(product =>
            product.Name.ToLower().Trim() == name.ToLower().Trim()
        );

        // Si el producto no existe o no hay suficiente stock, la compra no puede realizarse
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
        if (product == null)
            return false;

        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        _db.Products.Add(product);
        return Save();
    }

    /// <summary>
    /// Elimina un  producto
    /// </summary>
    /// <param name="product">Producto a eliminar</param>
    /// <returns>Metodo Save para guardar cambios</returns>
    public bool DeleteProduct(Product product)
    {
        if (product == null)
            return false;

        _db.Products.Remove(product);
        return Save();
    }

    /// <summary>
    ///  Obtiene un producto por su ID
    /// </summary>
    /// <param name="id">ID del producto</param>
    /// <returns>El producto si existe, null en caso contrario</returns>
    public Product? GetProduct(int id)
    {
        if (id <= 0)
            return null;

        return _db.Products
            .Include(product => product.Category)
            .FirstOrDefault(product => product.Id == id);
    }

    /// <summary>
    /// Obtiene todos los productos ordenados por nombre
    /// </summary>
    /// <returns>Una colección de productos</returns>    
    public ICollection<Product> GetProducts()
    {
        return _db.Products
            .OrderBy(product => product.Name)
            .Include(product => product.Category)
            .ToList();
    }

    /// <summary>
    /// Obtiene los productos de una categoría específica
    /// </summary>
    /// <param name="categoryId">ID de la categoría</param>
    /// <returns>Una colección de productos que pertenecen a la categoría</returns>
    public ICollection<Product> GetProductsForCategory(int categoryId)
    {
        if (categoryId <= 0)
            return new List<Product>();

        return _db.Products.Where(product => product.CategoryId == categoryId).ToList();
    }

    /// <summary>
    /// Determina si un producto existe
    /// </summary>
    /// <param name="id">id para buscar producto</param>
    /// <returns>True si un producto existe</returns>
    public bool ProductExists(int id)
    {
        if (id <= 0)
            return false;

        return _db.Products.Any(product => product.Id == id);
    }

/// <summary>
/// Verifica si un producto existe por nombre
/// </summary>
/// <param name="name">nombre a buscar</param>
/// <returns>true si el producto existe</returns>
    public bool ProductExists(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        return _db.Products.Any(product => product.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    /// <summary>
    /// Guarda cambios en la base de datos
    /// </summary>
    /// <returns>true si se guardo satisfactoriamente</returns>
    public bool Save()
    {
        // SaveChanges devuelve el número de objetos escritos en la base de datos
        return _db.SaveChanges() >= 0;
    }

/// <summary>
/// Busca un producto en la base de datos
/// </summary>
/// <param name="name">nombre a buscar</param>
/// <returns>Una lista de productos</returns>
    public ICollection<Product> SearchProduct(string name)
    {
        IQueryable<Product> query = _db.Products;

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(product => product.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        return query.ToList();
    }

    /// <summary>
    /// Actualiza un producto en la base de datos
    /// </summary>
    /// <param name="product">producto a actualizar</param>
    /// <returns>Return description</returns>
    public bool UpdateProduct(Product product)
    {
        if (product == null)
            return false;

        product.UpdatedAt = DateTime.UtcNow;
        _db.Products.Update(product);
        return Save();
    }
}
