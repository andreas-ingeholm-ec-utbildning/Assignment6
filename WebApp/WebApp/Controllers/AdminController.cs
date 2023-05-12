using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers;

[Authorize(UserRole.Admin)]
public partial class AdminController : Controller
{

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

}
