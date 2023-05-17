using WebApp.Contexts;
using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services;

/// <summary>Manages categories.</summary>
public class CategoryService
{

    #region Injections

    readonly Repo<ProductCategoryEntity> categoryRepo;
    readonly DataContext context;

    public CategoryService(Repo<ProductCategoryEntity> categoryRepo, DataContext context)
    {
        this.categoryRepo = categoryRepo;
        this.context = context;
    }

    #endregion

    /// <summary>Gets or creates a category with the specified name.</summary>
    public async Task<ProductCategoryEntity> GetOrCreateAsync(string name) =>
        await categoryRepo.GetAsync(c => c.Name == name) ??
        await categoryRepo.AddAsync(new() { Name = name });

    /// <summary>Enumerates all categories.</summary>
    public async Task<IEnumerable<ProductCategory>> EnumerateAsync() =>
        (await categoryRepo.EnumerateAsync()).Select(c => (ProductCategory)c);

    /// <summary>Gets the category with the specified <paramref name="id"/>.</summary>
    public async Task<ProductCategoryEntity?> GetAsync(Guid id) =>
        await categoryRepo.GetAsync(c => c.ID == id);

    /// <summary>Creates a category.</summary>
    public async Task<ProductCategoryEntity> CreateAsync(CategoryFormView view) =>
        await GetOrCreateAsync(view.Name);

    /// <summary>Updates a category.</summary>
    public async Task<bool> Update(CategoryFormView view)
    {

        var category = await GetAsync(view.ID);
        if (category is not null)
        {
            category.Name = view.Name;
            _ = await context.SaveChangesAsync();
            return true;
        }

        return false;

    }

    /// <summary>Deletes the category with the specified <paramref name="id"/>.</summary>
    public async Task<bool> Delete(Guid id)
    {
        if (await GetAsync(id) is ProductCategoryEntity category)
        {
            await categoryRepo.DeleteAsync(category);
            return true;
        }
        else
            return false;
    }

}
