using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Entities;
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

    [Route("admin/{*url}", Order = 999)]
    public IActionResult Index() =>
        View();

    #region Product

    [HttpGet("product/add")]
    public async Task<IActionResult> AddProduct() =>
        View(new ProductAddView() { Categories = await categoryService.EnumerateAsync() });

    [HttpPost("product/add")]
    public async Task<IActionResult> AddProduct(ProductAddView view)
    {

        if (ModelState.IsValid)
        {
            var product = await productService.CreateAsync(view.Form);
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

    #endregion
    #region Category

    [HttpGet("category/add")]
    public IActionResult AddCategory() =>
        View();

    [HttpPost("category/add")]
    public async Task<IActionResult> AddCategory(CategoryAddView view)
    {

        if (ModelState.IsValid)
        {
            var category = await categoryService.CreateAsync(view);
            if (category is not null)
                return RedirectToAction("category", routeValues: new { id = category.ID });
            else
                ModelState.AddModelError("", "Something went wrong when creating category.");
        }

        return View();

    }

    [HttpGet]
    public async Task<IActionResult> Category(Guid id)
    {

        if (await categoryService.GetAsync(id) is ProductCategoryEntity category)
            return View((CategoryEditView)category);

        ModelState.AddModelError("", "Could not find category.");
        return RedirectToAction("index");

    }

    [HttpPost]
    public async Task<IActionResult> Category(CategoryEditView view)
    {

        if (!ModelState.IsValid || !await categoryService.Update(view))
            ModelState.AddModelError("", "An error occured when updating category.");

        return View(view);

    }

    [HttpGet("category")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {

        if (await categoryService.Delete(id))
            return RedirectToAction("index");
        else
        {
            ModelState.AddModelError("", "An error occured when deleting category.");
            return View();
        }

    }

    #endregion
    #region User

    [HttpGet]
    public new async Task<IActionResult> User(Guid id)
    {

        var user = await userService.GetAsync(id);
        if (user is null)
            return StatusCode(404);

        var view = (UserEditAdminView)user;
        view.Role = await authService.GetRoleAsync(user.User);

        if (base.User.Identity?.Name is string email)
            view.IsLoggedIn = userService.Get(email)?.UserID == id;

        return View(view);

    }

    [HttpPost]
    public new async Task<IActionResult> User(UserEditAdminView view)
    {

        if (!ModelState.IsValid || !await userService.UpdateAsync(view))
            ModelState.AddModelError("", "Something went wrong when updating user.");

        return View(view);

    }

    #endregion

}
