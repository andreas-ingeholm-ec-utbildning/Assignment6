using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Entities;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public partial class AdminController
{

    [HttpGet("admin/tag/add")]
    public IActionResult AddTag() =>
        View();

    [HttpPost("admin/tag/add")]
    public async Task<IActionResult> AddTag(TagAddView view)
    {

        if (ModelState.IsValid)
        {
            var tag = await tagService.CreateAsync(view);
            if (tag is not null)
                return RedirectToAction("tag", routeValues: new { id = tag.ID });
            else
                ModelState.AddModelError("", "Something went wrong when creating tag.");
        }

        return View();

    }

    [HttpGet("admin/tag")]
    public async Task<IActionResult> Tag(Guid id)
    {

        if (await tagService.GetAsync(id) is TagEntity tag)
            return View((TagEditView)tag);
        else
            return NotFound();
    }

    [HttpPost("admin/tag")]
    public async Task<IActionResult> Tag(TagEditView view)
    {

        if (!ModelState.IsValid || !await tagService.Update(view))
            ModelState.AddModelError("", "An error occured when updating tag.");

        return View(view);

    }

    [HttpGet("admin/tag/delete")]
    public async Task<IActionResult> DeleteTag(Guid id)
    {

        if (await tagService.Delete(id))
            return RedirectToAction("index");
        else
        {
            ModelState.AddModelError("", "An error occured when deleting tag.");
            return View();
        }

    }

}
