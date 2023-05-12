using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public partial class AdminController
{

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

        //Make sure page reloads for user, otherwise claims won't be refreshed
        return Redirect(view.ID.ToString());

    }

}
