using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize(UserRole.Admin)]
public class AdminController : Controller
{

    readonly AuthService authService;
    readonly UserService userService;
    readonly ProductService productService;
    readonly CategoryService categoryService;

    public AdminController(ProductService productService, CategoryService categoryService, UserService userService, AuthService authService)
    {
        this.productService = productService;
        this.categoryService = categoryService;
        this.userService = userService;
        this.authService = authService;
    }

    [Route("Admin")]
    public IActionResult Index() =>
        View();

    #region User

    [HttpGet]
    public new async Task<IActionResult> User(Guid id)
    {

        var user = await userService.GetProfileEntityAsync(id);
        if (user is null)
            return StatusCode(404);

        var view = (UserEditAdminView)user;
        view.Role = await authService.GetRoleAsync(user.User);

        if (base.User.Identity?.Name is string email)
            view.IsLoggedIn = userService.GetProfileEntity(email)?.UserID == id;

        return View(view);

    }

    [HttpPost]
    public new async Task<IActionResult> User(UserEditAdminView view)
    {

        if (!ModelState.IsValid || !await userService.Update(view))
            ModelState.AddModelError("", "Something went wrong when updating user.");

        return View(view);

    }

    #endregion
    #region Product

    [HttpGet("product/add")]
    public async Task<IActionResult> AddProduct() =>
        View(new ProductAddView() { Categories = await categoryService.EnumerateCategoriesAsync() });

    [HttpPost("product/add")]
    public async Task<IActionResult> AddProduct(ProductAddView view)
    {

        if (ModelState.IsValid)
        {
            var product = await productService.CreateAsync(view.Form);
            if (product is not null)
                return RedirectToAction("User", routeValues: product.ID);
            else
                ModelState.AddModelError("", "Something went wrong when creating product.");
        }

        return View(view);

    }

    [HttpGet]
    public async Task<IActionResult> Product(Guid id)
    {

        var product = await productService.FindProduct(id);

        if (product is not null)
            return View(product);
        else
            return View();

    }

    [HttpPost]
    public IActionResult Product(ProductAddView view)
    {
        return View();
    }

    [HttpDelete("Product")]
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

    #endregion
    #region Category

    [HttpGet]
    public IActionResult Category(Guid id)
    {
        return View();
    }

    [HttpPost]
    public IActionResult Category(CategoryEditView view)
    {
        return View();
    }

    [HttpDelete("Category")]
    public IActionResult DeleteCategory(Guid id)
    {
        return View();
    }

    #endregion

}
