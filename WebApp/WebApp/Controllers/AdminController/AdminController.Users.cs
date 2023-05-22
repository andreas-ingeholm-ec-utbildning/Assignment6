using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public partial class AdminController
{

    #region Add

    [HttpGet("/admin/user/add")]
    public IActionResult AddUser() =>
        View();

    [HttpPost("/admin/user/add")]
    public async Task<IActionResult> AddUser(UserRegisterView view)
    {

        if (!ModelState.IsValid && !await authService.ExistsAsync(view.Email))
        {
            ModelState.AddModelError("", "Something went wrong when adding user.");
            return View(view);
        }

        var user = await authService.RegisterAsync(view);
        if (user is null)
        {
            ModelState.AddModelError("", "Something went wrong when adding user.");
            return View(view);
        }

        return RedirectToAction("user", new { id = user.Id });

    }

    #endregion
    #region Edit

    [HttpGet("admin/user")]
    public new async Task<IActionResult> User(Guid id)
    {

        var user = await userService.GetAsync(id);
        if (user is null)
            return StatusCode(404);

        var view = (UserFormAdminView)user;
        view.Role = await authService.GetRoleAsync(user.User);

        if (base.User.Identity?.Name is string email)
            view.IsLoggedIn = userService.Get(email)?.UserID == id;

        return View(view);

    }

    [HttpPost("admin/user")]
    public new async Task<IActionResult> User(UserFormAdminView view)
    {

        if (!ModelState.IsValid || !await userService.UpdateAsync(view))
            ModelState.AddModelError("", "Something went wrong when updating user.");

        //Make sure page reloads for user, otherwise claims won't be refreshed
        return Redirect(view.ID.ToString());

    }

    #endregion
    #region Delete

    [HttpGet("admin/user/delete")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {

        if (await userService.Delete(id))
            return RedirectToAction("index");
        else
        {
            ModelState.AddModelError("", "An error occured when deleting user.");
            return View();
        }

    }

    #endregion

}
