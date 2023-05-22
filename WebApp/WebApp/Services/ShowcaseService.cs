using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services;

/// <summary>Manages showcases.</summary>
public class ShowcaseService
{

    //private readonly Showcase[] showcases = new Showcase[]
    //{
    //    new()
    //    {
    //        Ingress = "WELCOME TO BMEKETO SHOP",
    //        Title = "Exclusive chair gold collection",
    //        ImageUrl = "images/placeholders/625x647.svg",
    //        LinkButton = new()
    //        {
    //            Text = "SHOP NOW",
    //            Url = "/products",
    //        }
    //    },
    //};

    #region Injections

    readonly Repo<ShowcaseEntity> repo;
    readonly IWebHostEnvironment webHostEnvironment;
    readonly ImageService imageService;

    public ShowcaseService(Repo<ShowcaseEntity> repo, IWebHostEnvironment webHostEnvironment, ImageService imageService)
    {
        this.repo = repo;
        this.webHostEnvironment = webHostEnvironment;
        this.imageService = imageService;
    }

    #endregion

    public async Task<Showcase?> GetAsync(Guid id)
    {
        var showcase = await repo.GetAsync(s => s.ID == id);
        return
            showcase is not null
            ? (Showcase)showcase
            : null;
    }

    public async Task<Showcase?> CreateAsync(ShowcaseFormView view)
    {

        var entity = await repo.AddAsync((ShowcaseEntity)view);

        entity.ImageUrl = await imageService.UploadImage<Showcase>(view.Image, entity.ID);
        await repo.UpdateAsync(entity);

        return entity;

    }

    public async Task<bool> UpdateAsync(ShowcaseFormView view)
    {

        var showcase = await repo.GetAsync(s => s.ID == view.ID);
        if (showcase is null)
            return false;

        showcase.IsVisible = view.IsVisible;
        showcase.Ingress = view.Ingress;
        showcase.Title = view.Title;
        showcase.LinkUrl = view.LinkUrl;
        showcase.LinkText = view.LinkText;

        if (view.Image is not null)
            showcase.ImageUrl = await imageService.UploadImage<Showcase>(view.Image, showcase.ID);

        await repo.UpdateAsync(showcase);
        view.CurrentImageUrl = showcase.ImageUrl;

        return true;

    }

    public async Task<bool> DeleteAsync(Guid id)
    {

        var showcase = await repo.GetAsync(s => s.ID == id);
        if (showcase is null)
            return false;

        await repo.DeleteAsync(showcase);
        return true;

    }

    public async Task<IEnumerable<Showcase>> EnumerateAsync() =>
        (await repo.EnumerateAsync()).Select(s => (Showcase)s);

}
