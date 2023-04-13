using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize(UserRole.Admin)]
public class ManageUsersController : Controller
{

    [Route("manage/users")]
    public IActionResult Index() =>
        View();

    [Route("manage/user")]
    public async Task<IActionResult> View(Guid id)
    {

        return RedirectToAction("Users");

    }

    [HttpPost]
    [Route("manage/user")]
    public async Task<IActionResult> Update(ProductEditView data)
    {
        return View(data);

    }

    [HttpPost]
    [Route("manage/user/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {

        return RedirectToAction("user", new { id });

    }

}
