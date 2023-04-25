using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Repositories;

namespace WebApp.Services;

public class CategoryService
{

    readonly Repo<ProductCategoryEntity> categoryRepo;

    public CategoryService(Repo<ProductCategoryEntity> categoryRepo) =>
        this.categoryRepo = categoryRepo;

    public async Task<ProductCategoryEntity> GetOrCreateAsync(ProductCategory category) =>
        await categoryRepo.GetAsync(c => c.ID == category.Value) ??
        await categoryRepo.AddAsync(new() { Name = category.Name });

    public async Task<IEnumerable<ProductCategory>> EnumerateCategoriesAsync() =>
        (await categoryRepo.EnumerateAsync()).Select(c => new ProductCategory() { Name = c.Name, Value = c.ID });

}
