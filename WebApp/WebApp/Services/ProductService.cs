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
    readonly TagService tagService;
    readonly IWebHostEnvironment webHostEnvironment;

    public ProductService(Repo<ProductEntity> productRepo, IWebHostEnvironment webHostEnvironment, TagService tagService, DataContext context)
    {
        this.productRepo = productRepo;
        this.webHostEnvironment = webHostEnvironment;
        this.tagService = tagService;
        this.context = context;
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
    public async Task<ProductEntity?> CreateAsync(ProductAddView view)
    {

        try
        {

            var entity = (ProductEntity)view;

            var product = await productRepo.AddAsync(entity);

            if (view.Image is not null && await UploadImage(product, view.Image))
                await productRepo.UpdateAsync(product);

            return product;

        }
        catch
        {
            return null;
        }

    }

    /// <summary>Updates a product.</summary>
    public async Task<bool> UpdateAsync(ProductEditView view)
    {

        var product = await productRepo.GetAsync(p => p.ID == view.ID);
        if (product is null)
            return false;

        product.Name = view.Name;
        product.Description = view.Description;
        product.Price = view.Price;
        product.CategoryID = view.Category;

        if (view.Image is not null)
            if (await UploadImage(product, view.Image))
                view.ExistingImageUrl = product.ImageUrl;
            else
                return false;

        await productRepo.UpdateAsync(product);

        return true;

    }

    /// <summary>Saves an image to the server and sets <see cref="ProductEntity.ImageUrl"/>.</summary>
    /// <remarks><see cref="UpdateAsync(ProductEditView)"/> needs to be called afterwards to save <see cref="ProductEntity.ImageUrl"/>.</remarks>
    async Task<bool> UploadImage(ProductEntity product, IFormFile image)
    {

        if (image is null)
            return false;

        try
        {

            var imageUrl = $"/images/products/{product.ID}_{image.FileName}";
            product.ImageUrl = imageUrl;

            var imagePath = $"{webHostEnvironment.WebRootPath}{product.ImageUrl}";
            using var fs = new FileStream(imagePath, FileMode.Create, FileAccess.Write);

            await image.CopyToAsync(fs);
            return true;

        }
        catch (Exception)
        {
            return false;
        }

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