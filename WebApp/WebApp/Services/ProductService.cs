using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;
using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services;

/// <summary>Manages products.</summary>
public class ProductService
{

    #region Injections

    readonly Repo<ProductEntity> productRepo;
    readonly DataContext context;
    readonly IWebHostEnvironment webHostEnvironment;
    readonly ImageService imageService;

    public ProductService(Repo<ProductEntity> productRepo, IWebHostEnvironment webHostEnvironment, DataContext context, ImageService imageService)
    {
        this.productRepo = productRepo;
        this.webHostEnvironment = webHostEnvironment;
        this.context = context;
        this.imageService = imageService;
    }

    #endregion

    /// <summary>Enumerates all products.</summary>
    public async Task<IEnumerable<Product>> EnumerateAsync() =>
        (await context.Products.Include(p => p.Tags).ThenInclude(t => t.Tag).ToArrayAsync()).Select(p => (Product)p);

    /// <summary>Finds a product with the specified <paramref name="id"/>.</summary>
    public async Task<Product?> FindProduct(Guid id)
    {
        var entity = (await context.Products.Include(p => p.Tags).ThenInclude(t => t.Tag).ToArrayAsync()).FirstOrDefault(p => p.ID == id);
        return
            entity is not null
            ? (Product)entity
            : null;
    }

    /// <summary>Creates a product.</summary>
    public async Task<ProductEntity?> CreateAsync(ProductFormView view)
    {

        try
        {

            var product = await productRepo.AddAsync((ProductEntity)view);

            product.ImageUrl = await imageService.UploadImage<Product>(view.Image, product.ID);
            await productRepo.UpdateAsync(product);

            return product;

        }
        catch
        {
            return null;
        }

    }

    /// <summary>Updates a product.</summary>
    public async Task<bool> UpdateAsync(ProductFormView view)
    {

        var product = await productRepo.GetAsync(p => p.ID == view.ID);
        if (product is null)
            return false;

        product.IsVisible = view.IsVisible;
        product.Name = view.Name;
        product.Description = view.Description;
        product.Price = view.Price;
        product.CategoryID = view.Category;

        if (view.Image is not null)
            product.ImageUrl = await imageService.UploadImage<Product>(view.Image, product.ID);

        await productRepo.UpdateAsync(product);
        view.CurrentImageUrl = product.ImageUrl;

        return true;

    }

    /// <summary>Deletes a product with the specified <paramref name="id"/>.</summary>
    public async Task<bool> DeleteAsync(Guid id)
    {

        var entity = await productRepo.GetAsync(p => p.ID == id);
        if (entity is null)
            return false;

        await productRepo.DeleteAsync(entity);

        return true;

    }

    /// <summary>Searches for products matching query specified in <paramref name="view"/>.</summary>
    public async Task Search(SearchView view)
    {

        var q = view.Query?.ToLower();

        view.Products =
            (await EnumerateAsync()).
            Where(Match);

        bool Match(Product product) =>
           string.IsNullOrWhiteSpace(q) || MatchesName(product) || MatchesCategory(product) || MatchesTag(product);

        bool MatchesName(Product product) =>
            product.Name.ToLower().Contains(q);

        bool MatchesCategory(Product product) =>
            product.Category?.Name?.ToLower()?.Contains(q) ?? false;

        bool MatchesTag(Product product) =>
            product.Tags?.Any(t => t.Name.ToLower().Contains(q)) ?? false;

    }

}