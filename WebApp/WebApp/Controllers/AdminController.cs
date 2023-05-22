using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Services;

namespace WebApp.Controllers;

/// <summary>Represents admin console page. Functionality is divided into separate files.</summary>
[Authorize(UserRole.Admin)]
public partial class AdminController : Controller
{

    readonly AuthService authService;
    readonly UserService userService;
    readonly ProductService productService;
    readonly CategoryService categoryService;
    readonly TagService tagService;
    readonly ShowcaseService showcaseService;

    public AdminController(ProductService productService, CategoryService categoryService, UserService userService, AuthService authService, TagService tagService, ShowcaseService showcaseService)
    {
        this.productService = productService;
        this.categoryService = categoryService;
        this.userService = userService;
        this.authService = authService;
        this.tagService = tagService;
        this.showcaseService = showcaseService;
    }

    [Route("admin/{*url}", Order = 999)]
    public IActionResult Index() =>
        View();

    public override void OnActionExecuting(ActionExecutingContext context)
    {

        //Used to populate sidebar
        ViewBag.Products = productService.EnumerateAsync().Result;
        ViewBag.Users = userService.EnumerateAsync().Result;
        ViewBag.Categories = categoryService.EnumerateAsync().Result;
        ViewBag.Tags = tagService.EnumerateAsync().Result;
        ViewBag.Showcases = showcaseService.EnumerateAsync().Result;

        base.OnActionExecuting(context);

    }

}
