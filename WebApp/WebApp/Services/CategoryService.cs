using WebApp.Contexts;
using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services;

public class CategoryService
{

    readonly Repo<ProductCategoryEntity> categoryRepo;
    readonly DataContext context;

    public CategoryService(Repo<ProductCategoryEntity> categoryRepo, DataContext context)
    {
        this.categoryRepo = categoryRepo;
        this.context = context;
    }

    public async Task<ProductCategoryEntity> GetOrCreateAsync(ProductCategory category) =>
        await categoryRepo.GetAsync(c => c.Name == category.Name) ??
        await categoryRepo.AddAsync(new() { Name = category.Name });

    public async Task<IEnumerable<ProductCategory>> EnumerateAsync() =>
        (await categoryRepo.EnumerateAsync()).Select(c => new ProductCategory() { Name = c.Name, Value = c.ID });

    public async Task<ProductCategoryEntity?> GetAsync(Guid guid) =>
        await categoryRepo.GetAsync(c => c.ID == guid);

    public async Task<ProductCategoryEntity> CreateAsync(CategoryAddView view) =>
        await GetOrCreateAsync(new() { Name = view.Name });

    public async Task<bool> Update(CategoryEditView view)
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
