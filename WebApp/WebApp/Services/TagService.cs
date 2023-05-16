using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Contexts;
using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Repositories;
using WebApp.ViewModels;

namespace WebApp.Services;

/// <summary>Manages tags.</summary>
public class TagService
{

    #region Injections

    readonly Repo<TagEntity> tagRepo;
    readonly Repo<ProductTagEntity> productTagRepo;
    readonly DataContext context;

    public TagService(Repo<TagEntity> tagRepo, DataContext context, Repo<ProductTagEntity> productTagRepo)
    {
        this.tagRepo = tagRepo;
        this.context = context;
        this.productTagRepo = productTagRepo;
    }

    #endregion
    #region Tags

    #region Enumerate

    /// <summary>Enumerates all tags.</summary>
    public async Task<IEnumerable<Tag>> EnumerateAsync() =>
        (await tagRepo.EnumerateAsync()).Select(t => (Tag)t);

    /// <summary>Enumerates all tags as <see cref="SelectListItem"/>, with <see cref="SelectListItem.Selected"/> set to <see langword="true"/> if <paramref name="product"/> has tag.</summary>
    public async Task<IEnumerable<SelectListItem>> EnumerateFromAsync(Product? product = null) =>
        (await tagRepo.EnumerateAsync()).Select(tag => new SelectListItem() { Text = tag.Name, Value = tag.ID.ToString(), Selected = product?.Tags?.Any(t => t.TagID == tag.ID) ?? false });

    /// <inheritdoc cref="EnumerateFromAsync(Guid[])"/>
    public async Task<IEnumerable<SelectListItem>> EnumerateFromAsync(string[] tagIDs) =>
       await EnumerateFromAsync(tagIDs.Select(Guid.Parse).ToArray());

    /// <summary>Enumerates all tags as <see cref="SelectListItem"/>, with <see cref="SelectListItem.Selected"/> set to <see langword="true"/> if <paramref name="tagIDs"/> contains it.</summary>
    public async Task<IEnumerable<SelectListItem>> EnumerateFromAsync(Guid[] tagIDs) =>
        (await tagRepo.EnumerateAsync()).Select(tag => new SelectListItem() { Text = tag.Name, Value = tag.ID.ToString(), Selected = tagIDs.Any(id => id == tag.ID) });

    #endregion

    /// <summary>Gets the tag with the specified tag, if one exists.</summary>
    public async Task<TagEntity?> GetAsync(Guid guid) =>
        await tagRepo.GetAsync(c => c.ID == guid);

    /// <summary>Creates a tag.</summary>
    public async Task<TagEntity> CreateAsync(TagAddView view) =>
        await tagRepo.AddAsync(new() { Name = view.Name });

    /// <summary>Updates a tag.</summary>
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

    /// <summary>Deletes a tag.</summary>
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

    #endregion
    #region Product tags

    /// <inheritdoc cref="AssignTagsAsync(Guid, Guid[])"/>
    public async Task AssignTagsAsync(Guid productID, string[] tags) =>
        await AssignTagsAsync(productID, tags.Select(Guid.Parse).ToArray());

    /// <summary>Assign <paramref name="tags"/> to the product with <paramref name="productID"/>.</summary>
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

    #endregion

}
