using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public partial class AdminController
{

    [HttpGet("product/add")]
    public async Task<IActionResult> AddProduct()
    {

        ViewBag.Categories = await categoryService.EnumerateAsync();
        ViewBag.Tags = await tagService.EnumerateFromNewProductAsync();

        return View();

    }

    [HttpPost("product/add")]
    public async Task<IActionResult> AddProduct(ProductAddView view)
    {

        if (ModelState.IsValid)
        {
            var product = await productService.CreateAsync(view, ViewBag.Tags);
            if (product is not null)
                return RedirectToAction("user", routeValues: product.ID);
            else
                ModelState.AddModelError("", "Something went wrong when creating product.");
        }

        return View(view);

    }

    [HttpGet]
    public async Task<IActionResult> Product(Guid id)
    {
        return View();
        //var product = await productService.FindProduct(id);
        //if (product is null)
        //    return NotFound();

        //ViewBag.Categories = await categoryService.EnumerateAsync();
        //ViewBag.Tags = await tagService.EnumerateFromAsync(product);

        //return View((ProductEditView)product);

    }

    [HttpPost]
    public IActionResult Product(ProductAddView view)
    {
        return View(view);
    }

    [HttpDelete("product")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {

        var product = await productService.FindProduct(id);

        if (product is not null)
        {

            return RedirectToAction("Index");
        }
        else
            return View();

    }

}

