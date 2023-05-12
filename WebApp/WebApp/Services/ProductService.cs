using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services;

public class ProductService
{

    readonly Repo<ProductEntity> productRepo;

    public ProductService(Repo<ProductEntity> productRepo) =>
        this.productRepo = productRepo;

    public async Task<IEnumerable<ProductEntity>> EnumerateAsync() =>
        await productRepo.EnumerateAsync();

    public async Task<Product?> FindProduct(Guid id) =>
        await productRepo.GetAsync(p => p.ID == id);

    public async Task<ProductEntity?> CreateAsync(ProductAddView view)
    {

        try
        {
            var entity = (ProductEntity)view;
            return await productRepo.AddAsync(entity);
        }
        catch (Exception e)
        {
            return null;
        }

    }

    public async Task<bool> UpdateAsync(ProductEditView view)
    {

        var product = await productRepo.GetAsync(p => p.ID == view.ID);
        if (product is null)
            return false;

        product.Name = view.Name;
        product.Description = view.Description;
        product.Price = view.Price;
        product.CategoryID = view.Category;

        await productRepo.UpdateAsync(product);

        return true;

    }

    public async Task<bool> DeleteAsync(Guid id)
    {

        var entity = await productRepo.GetAsync(p => p.ID == id);
        if (entity is null)
            return false;

        await productRepo.DeleteAsync(entity);

        return true;

    }

}