
using ApiEcommerce.Models;
using ApiEcommerce.Repositories.Interfaces;

namespace ApiEcommerce.Repositories;

public class CategoryRepository : ICategoryRepository
{

    private readonly ApplicationDbContext _db;

    public CategoryRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public bool CategoryExists(int id) =>
        _db.Categories.Any(category => category.Id == id);


    public bool CategoryExists(string name) =>
        _db.Categories.Any(category => category.Name.ToLower() == name.ToLower().Trim());



    public bool CreateCategory(Category category)
    {
        category.CreationDate = DateTime.Now;
        _db.Categories.Add(category);
        return Save();
    }

    public bool DeleteCategory(Category category)
    {
        _db.Categories.Remove(category);
        return Save();
    }

    public ICollection<Category> GetCategories() =>
        _db.Categories.OrderBy(category => category.Name).ToList();


    public Category GetCategory(int id) =>
        _db.Categories.FirstOrDefault(category => category.Id == id) ?? throw new InvalidOperationException($"La categoria con el id {id} no existe");

    public bool Save() => _db.SaveChanges() >= 0 ? true : false;

    public bool UpdateCategory(Category category)
    {
        category.CreationDate = DateTime.Now;
        _db.Categories.Update(category);
        return Save();
    }
}
