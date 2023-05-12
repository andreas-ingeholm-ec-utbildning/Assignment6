using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Contexts;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services;

public class TagService
{

    readonly Repo<TagEntity> tagRepo;
    readonly DataContext context;

    public TagService(Repo<TagEntity> tagRepo, DataContext context)
    {
        this.tagRepo = tagRepo;
        this.context = context;
    }

    public async Task<TagEntity> GetOrCreateAsync(string name) =>
        await tagRepo.GetAsync(c => c.Name == name) ??
        await tagRepo.AddAsync(new() { Name = name });

    public async Task<IEnumerable<TagEntity>> EnumerateAsync() =>
        (await tagRepo.EnumerateAsync()).Select(c => new TagEntity() { Name = c.Name, ID = c.ID });

    //public async Task<IEnumerable<SelectListItem>> EnumerateFromAsync(Product product) =>
    //    (await EnumerateAsync()).Select(t => new SelectListItem() { Value = t.ID.ToString(), Text = t.Name }).ToArray();

    public async Task<IEnumerable<SelectListItem>> EnumerateFromNewProductAsync() =>
        (await EnumerateAsync()).Select(t => new SelectListItem() { Value = t.ID.ToString(), Text = t.Name }).ToArray();

    public async Task<TagEntity?> GetAsync(Guid guid) =>
        await tagRepo.GetAsync(c => c.ID == guid);

    public async Task<TagEntity> CreateAsync(TagAddView view) =>
        await GetOrCreateAsync(view.Name);

    public async Task<bool> Update(TagEditView view)
    {

        var tag = await GetAsync(view.ID);
        if (tag is not null)
        {
            tag.Name = view.Name;
            _ = await context.SaveChangesAsync();
            return true;
        }

        return false;

    }

    public async Task<bool> Delete(Guid id)
    {
        if (await GetAsync(id) is TagEntity tag)
        {
            await tagRepo.DeleteAsync(tag);
            return true;
        }
        else
            return false;
    }

}
