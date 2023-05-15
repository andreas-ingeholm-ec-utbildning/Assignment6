using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Contexts;
using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services;

public class TagService
{

    readonly Repo<TagEntity> tagRepo;
    readonly Repo<ProductTagEntity> productTagRepo;
    readonly DataContext context;

    public TagService(Repo<TagEntity> tagRepo, DataContext context, Repo<ProductTagEntity> productTagRepo)
    {
        this.tagRepo = tagRepo;
        this.context = context;
        this.productTagRepo = productTagRepo;
    }

    public async Task<IEnumerable<Tag>> EnumerateAsync() =>
        (await tagRepo.EnumerateAsync()).Select(t => (Tag)t);

    public async Task<IEnumerable<SelectListItem>> EnumerateFromAsync(Product? product = null) =>
        (await tagRepo.EnumerateAsync()).Select(tag => new SelectListItem() { Text = tag.Name, Value = tag.ID.ToString(), Selected = product?.Tags?.Any(t => t.TagID == tag.ID) ?? false });

    public async Task<IEnumerable<SelectListItem>> EnumerateFromAsync(Guid[] tagIDs) =>
        (await tagRepo.EnumerateAsync()).Select(tag => new SelectListItem() { Text = tag.Name, Value = tag.ID.ToString(), Selected = tagIDs.Any(id => id == tag.ID) });

    public async Task<IEnumerable<SelectListItem>> EnumerateFromAsync(string[] tagIDs) =>
       await EnumerateFromAsync(tagIDs.Select(Guid.Parse).ToArray());

    public async Task<TagEntity?> GetAsync(Guid guid) =>
        await tagRepo.GetAsync(c => c.ID == guid);

    public async Task<TagEntity> CreateAsync(TagAddView view) =>
        await tagRepo.AddAsync(new() { Name = view.Name });

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

    public async Task AssignTagsAsync(Guid productID, string[] tags) =>
        await AssignTagsAsync(productID, tags.Select(Guid.Parse).ToArray());

    public async Task AssignTagsAsync(Guid productID, Guid[] tags)
    {

        var allTags = (await productTagRepo.EnumerateAsync()).Where(t => t.ProductID == productID);
        var tagsToRemove = allTags.Where(t => !tags.Contains(t.TagID));
        var tagsToAdd = tags.Where(t => !allTags.Any(t2 => t2.TagID == t));

        foreach (var tag in tagsToRemove)
            await productTagRepo.DeleteAsync(tag);

        foreach (var tag in tagsToAdd)
            _ = await productTagRepo.AddAsync(new()
            {
                ProductID = productID,
                TagID = tag
            });

    }

}
