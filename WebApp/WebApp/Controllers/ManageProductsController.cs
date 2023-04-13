using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize(UserRole.Admin)]
public class ManageProductsController : Controller
{

    [Route("manage/products")]
    public IActionResult Index() =>
        View();

    [Route("manage/product")]
    public async Task<IActionResult> View(Guid id)
    {

        //if (await productService.FindProduct(id) is Product product)
        //return View((ProductEditView)product);
        //else
        return RedirectToAction("Users");

    }

    [HttpPost]
    [Route("manage/product")]
    public async Task<IActionResult> Update(ProductEditView data)
    {

        //if (!await productService.EditAsync(data))
        //    ModelState.AddModelError("", "Something went wrong when editing product.");

        return View(data);

    }

    [HttpPost]
    [Route("manage/product/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {

        //if (await productService.DeleteAsync(id))
        //    return RedirectToAction("Manage");
        //else
        //{
        //    ModelState.AddModelError("", "Something went wrong when deleting product.");
        return RedirectToAction("product", new { id });
        //}

    }

}
