using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers;

public class HomeController : Controller
{

    readonly ShowcaseService showcaseService;
    readonly TagService tagService;

    public HomeController(ShowcaseService showcaseService, TagService tagService)
    {
        this.showcaseService = showcaseService;
        this.tagService = tagService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Showcases = await showcaseService.EnumerateAsync();
        ViewBag.Products = await tagService.EnumerateProductTagsAndProducts();
        return View();
    }

}
