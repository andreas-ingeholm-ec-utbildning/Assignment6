using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class ProductsController : Controller
{

    readonly ProductService productService;

    public ProductsController(ProductService productService) =>
        this.productService = productService;

    public async Task<IActionResult> Index()
    {
        ViewBag.Products = (await productService.EnumerateAsync());
        return View();
    }

    public async Task<IActionResult> Search()
    {
        var view = new SearchView();
        await productService.Search(view);
        return View(view);
    }

    [HttpPost]
    public async Task<IActionResult> Search(SearchView view)
    {
        await productService.Search(view);
        return View(view);
    }

    public IActionResult Product() =>
        View();

}
