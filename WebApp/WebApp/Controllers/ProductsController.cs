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

    public async Task<IActionResult> Search(string q)
    {
        var view = new SearchView() { Query = q };
        await productService.Search(view);
        return View(view);
    }

    [HttpPost]
    public async Task<IActionResult> Search(SearchView view)
    {
        await productService.Search(view);
        return View(view);
    }

    [Route("product/{id}")]
    public async Task<IActionResult> Product(Guid id)
    {
        var product = await productService.FindProduct(id);
        return
            product is not null
            ? View(product)
            : NotFound();
    }

}
