using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Entities;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public partial class AdminController
{

    #region Add

    [HttpGet("admin/category/add")]
    public IActionResult AddCategory() =>
        View();

    [HttpPost("admin/category/add")]
    public async Task<IActionResult> AddCategory(CategoryFormView view)
    {

        if (ModelState.IsValid)
        {
            var category = await categoryService.CreateAsync(view);
            if (category is not null)
                return RedirectToAction("category", routeValues: new { id = category.ID });
            else
                ModelState.AddModelError("", "Something went wrong when creating category.");
        }

        return View();

    }

    #endregion
    #region Edit

    [HttpGet("admin/category")]
    public async Task<IActionResult> Category(Guid id)
    {

        if (await categoryService.GetAsync(id) is ProductCategoryEntity category)
            return View((CategoryFormView)category);
        else
            return NotFound();

    }

    [HttpPost("admin/category")]
    public async Task<IActionResult> Category(CategoryFormView view)
    {

        if (!ModelState.IsValid || !await categoryService.Update(view))
            ModelState.AddModelError("", "An error occured when updating category.");

        return View(view);

    }

    #endregion
    #region Delete

    [HttpGet("admin/category/delete")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {

        if (await categoryService.Delete(id))
            return RedirectToAction("index");
        else
        {
            ModelState.AddModelError("", "An error occured when deleting category.");
            return View();
        }

    }

    #endregion

}
