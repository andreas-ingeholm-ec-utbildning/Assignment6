using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public partial class AdminController
{

    [HttpGet("product/add")]
    public IActionResult AddProduct() =>
        View();

    [HttpPost("product/add")]
    public async Task<IActionResult> AddProduct(ProductAddView view)
    {

        if (ModelState.IsValid)
        {

            var product = await productService.CreateAsync(view);

            if (product is not null)
                return RedirectToAction("product", routeValues: new { id = product.ID });
            else
                ModelState.AddModelError("", "Something went wrong when creating product.");

        }
        else
            ModelState.AddModelError("", "Something went wrong when creating product.");

        return View(view);

    }

    [HttpGet]
    public async Task<IActionResult> Product(Guid id)
    {

        var product = await productService.FindProduct(id);
        return
            product is not null
            ? View((ProductEditView)product)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProductAsync(ProductEditView view)
    {

        if (!ModelState.IsValid || !await productService.UpdateAsync(view))
            ModelState.AddModelError("", "Something went wrong when updating product.");

        return View(view);

    }

    [HttpGet("product")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        if (await productService.DeleteAsync(id))
            return RedirectToAction("Index");
        else
            return View();
    }

}

