using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services;

public class ProductService
{

    readonly CategoryService categoryService;
    readonly Repo<ProductEntity> productRepo;

    public ProductService(CategoryService categoryService, Repo<ProductEntity> productRepo)
    {
        this.categoryService = categoryService;
        this.productRepo = productRepo;
    }

    public async Task<IEnumerable<ProductEntity>> EnumerateAsync() =>
        await productRepo.EnumerateAsync();

    public async Task<Product?> FindProduct(Guid id) =>
        await productRepo.GetAsync(p => p.ID == id);

    public async Task<Product?> CreateAsync(ProductAddView view, IEnumerable<SelectListItem>? tags = null)
    {

        try
        {

            var entity = (ProductEntity)view;
            entity.CategoryId = view.Category?.ID;

            return await productRepo.AddAsync(entity);

        }
        catch
        {
            return null;
        }

    }

    public async Task<bool> UpdateAsync(ProductEditView form)
    {

        //var entity = await context.Products.FirstOrDefaultAsync(p => p.ID == form.ID);
        //if (entity is null)
        //    return false;

        //entity.Name = form.Name;
        //entity.Description = form.Description;
        //entity.Price = form.Price;
        //entity.CategoryID = (await categoryService.GetOrCreateAsync(form.Category)).ID;

        //_ = await context.SaveChangesAsync();

        return true;

    }

    public async Task<bool> DeleteAsync(Guid id)
    {

        //var entity = await context.Products.FirstOrDefaultAsync(p => p.ID == id);
        //if (entity is null)
        //    return false;

        //_ = context.Remove(entity);
        //_ = await context.SaveChangesAsync();

        return true;

    }

}