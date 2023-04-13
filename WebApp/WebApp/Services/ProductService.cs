using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;
using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.ViewModels;

namespace WebApp.Services;

public class ProductService
{

    private readonly DataContext context;

    public ProductService(DataContext context) =>
        this.context = context;

    public async Task<Product?> CreateAsync(ProductRegisterView data)
    {

        try
        {

            var entity = (ProductEntity)data;
            _ = await context.Products.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            return entity;

        }
        catch
        {
            return null;
        }

    }

    public async Task<IEnumerable<Product>> EnumerateAsync() =>
         (await context.Products.ToArrayAsync()).Select(p => (Product?)p).OfType<Product>();

    public async Task<Product?> FindProduct(Guid id) =>
        await context.Products.FirstOrDefaultAsync(p => p.ID == id);

    public async Task<bool> EditAsync(ProductEditView data)
    {

        var entity = await context.Products.FirstOrDefaultAsync(p => p.ID == data.ID);
        if (entity is null)
            return false;

        entity.Name = data.Name ?? entity.Name;
        entity.Description = data.Description ?? entity.Description;
        entity.Price = data.Price ?? entity.Price;

        _ = await context.SaveChangesAsync();
        return true;

    }

    public async Task<bool> DeleteAsync(Guid id)
    {

        var entity = await context.Products.FirstOrDefaultAsync(p => p.ID == id);
        if (entity is null)
            return false;

        _ = context.Remove(entity);
        _ = await context.SaveChangesAsync();

        return true;

    }

}