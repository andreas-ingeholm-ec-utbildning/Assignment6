using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class ProductsController : Controller
{

    private readonly ProductService productService;

    public ProductsController(ProductService productService) =>
        this.productService = productService;

    public IActionResult Index()
    {
        ViewData["Title"] = "Products";
        return View();
    }

    public IActionResult Search()
    {
        ViewData["Title"] = "Search for products";
        return View();
    }

    [Authorize(Roles = UserRole.Admin)]
    public IActionResult Manage() =>
        View();

    #region Register

    [Authorize(Roles = UserRole.Admin)]
    public IActionResult Register() => View();

    [HttpPost]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Register(ProductRegisterView data)
    {

        if (ModelState.IsValid && await productService.CreateAsync(data) is Product product)
            return RedirectToAction("edit", "products", new { product.ID });
        ModelState.AddModelError("", "Something went wrong when creating product.");

        return View();

    }

    #endregion
    #region Edit

    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Edit(Guid id)
    {

        if (await productService.FindProduct(id) is Product product)
            return View((ProductEditView)product);
        else
            return RedirectToAction("Manage");

    }

    [HttpPost]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Edit(ProductEditView data)
    {

        if (!await productService.EditAsync(data))
            ModelState.AddModelError("", "Something went wrong when editing product.");

        return View(data);

    }

    #endregion
    #region Delete

    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {

        if (await productService.DeleteAsync(id))
            return RedirectToAction("Manage");
        else
        {
            ModelState.AddModelError("", "Something went wrong when deleting product.");
            return RedirectToAction("edit", "products", new { id });
        }

    }

    #endregion

}
