using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public partial class AdminController
{

    [HttpGet("admin/product/add")]
    public async Task<IActionResult> AddProduct()
    {
        ViewBag.ProductTags = await tagService.EnumerateFromAsync(product: null);
        return View();
    }

    [HttpPost("admin/product/add")]
    public async Task<IActionResult> AddProduct(ProductAddView view, string[] tags)
    {

        if (ModelState.IsValid)
        {

            var product = await productService.CreateAsync(view);
            if (product is not null)
            {
                await tagService.AssignTagsAsync(product.ID, tags);
                ViewBag.ProductTags = await tagService.EnumerateFromAsync(product);
                return RedirectToAction("product", routeValues: new { id = product.ID });
            }

        }

        ModelState.AddModelError("", "Something went wrong when creating product.");

        ViewBag.ProductTags = await tagService.EnumerateFromAsync(tags);
        return View(view);

    }

    [HttpGet("admin/product")]
    public async Task<IActionResult> Product(Guid id)
    {

        var product = await productService.FindProduct(id);
        if (product is null)
            return NotFound();

        ViewBag.ProductTags = tagService.EnumerateFromAsync(product).Result;
        return View((ProductEditView)product);

    }

    [HttpPost("admin/product")]
    public async Task<IActionResult> Product(ProductEditView view, string[] tags)
    {

        if (!ModelState.IsValid || !await productService.UpdateAsync(view))
            ModelState.AddModelError("", "Something went wrong when updating product.");
        else
            await tagService.AssignTagsAsync(view.ID, tags);

        ViewBag.ProductTags = await tagService.EnumerateFromAsync(tags);

        return View(view);

    }

    [HttpGet("admin/product/delete")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        if (await productService.DeleteAsync(id))
            return RedirectToAction("index");
        else
            return View();
    }

}

