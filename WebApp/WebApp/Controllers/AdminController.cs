using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Services;

namespace WebApp.Controllers;

[Authorize(UserRole.Admin)]
public partial class AdminController : Controller
{

    //TODO: Add tags
    //TODO: Add documentation

    //TODO: Fix home page
    //TODO: Fix products page
    //TODO: Fix product page
    //TODO: Fix contact form
    //TODO: Add shopping cart

    readonly AuthService authService;
    readonly UserService userService;
    readonly ProductService productService;
    readonly CategoryService categoryService;
    readonly TagService tagService;

    public AdminController(ProductService productService, CategoryService categoryService, UserService userService, AuthService authService, TagService tagService)
    {
        this.productService = productService;
        this.categoryService = categoryService;
        this.userService = userService;
        this.authService = authService;
        this.tagService = tagService;
    }

    [Route("admin/{*url}", Order = 999)]
    public IActionResult Index() =>
        View();

    public override void OnActionExecuting(ActionExecutingContext context)
    {

        //All of these needs to be sent for sidebar
        ViewBag.Products = productService.EnumerateAsync().Result;
        ViewBag.Users = userService.EnumerateAsync().Result;
        ViewBag.Categories = categoryService.EnumerateAsync().Result;
        ViewBag.Tags = tagService.EnumerateAsync().Result;

        base.OnActionExecuting(context);

    }

}
