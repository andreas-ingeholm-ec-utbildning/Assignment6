using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public partial class AdminController
{

    #region Add

    [HttpGet("admin/showcase/add")]
    public IActionResult AddShowcase() =>
        View();

    [HttpPost("admin/showcase/add")]
    public async Task<IActionResult> AddShowcase(ShowcaseFormView view)
    {

        if (ModelState.IsValid)
        {

            var showcase = await showcaseService.CreateAsync(view);
            if (showcase is not null)
                return RedirectToAction("showcase", routeValues: new { id = showcase.ID });

        }

        ModelState.AddModelError("", "Something went wrong when creating showcase.");

        return View(view);

    }

    #endregion
    #region Edit

    [HttpGet("admin/showcase")]
    public async Task<IActionResult> Showcase(Guid id)
    {

        var showcase = await showcaseService.GetAsync(id);
        if (showcase is null)
            return NotFound();

        return View((ShowcaseFormView)showcase);

    }

    [HttpPost("admin/showcase")]
    public async Task<IActionResult> Showcase(ShowcaseFormView view)
    {

        if (!ModelState.IsValid || !await showcaseService.UpdateAsync(view))
            ModelState.AddModelError("", "Something went wrong when updating showcase.");

        return View(view);

    }

    #endregion
    #region Delete

    [HttpGet("admin/showcase/delete")]
    public async Task<IActionResult> DeleteShowcase(Guid id)
    {
        if (await showcaseService.DeleteAsync(id))
            return RedirectToAction("index");
        else
            return View();
    }

    #endregion

}
