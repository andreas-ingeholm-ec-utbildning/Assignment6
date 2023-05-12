using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Entities;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public partial class AdminController
{

    [HttpGet("category/add")]
    public IActionResult AddCategory() =>
        View();

    [HttpPost("category/add")]
    public async Task<IActionResult> AddCategory(CategoryAddView view)
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

    [HttpGet]
    public async Task<IActionResult> Category(Guid id)
    {

        if (await categoryService.GetAsync(id) is ProductCategoryEntity category)
            return View((CategoryEditView)category);
        else
            return NotFound();

    }

    [HttpPost]
    public async Task<IActionResult> Category(CategoryEditView view)
    {

        if (!ModelState.IsValid || !await categoryService.Update(view))
            ModelState.AddModelError("", "An error occured when updating category.");

        return View(view);

    }

    [HttpGet("category")]
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

}
