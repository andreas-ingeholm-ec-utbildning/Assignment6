using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Identity;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Route("register")]
public class UserRegisterController : Controller
{

    private readonly UserManager<User> userManager;

    public UserRegisterController(UserManager<User> userManager) =>
        this.userManager = userManager;

    [HttpGet]
    public IActionResult Index() =>
        View(new UserRegisterView());

    [HttpPost]
    public async Task<IActionResult> Index(UserRegisterView view)
    {

        if (ModelState.IsValid)
            if (await userManager.FindByEmailAsync(view.Email) is not null)
                ModelState.AddModelError("", "A user with the specified email address already exists.");
            else
            {
                var result = await userManager.CreateAsync(view, view.Password);
                if (!result.Succeeded)
                    ModelState.AddModelError("", string.Join("\n", result.Errors.Select(e => e.Description)));
                else
                    return RedirectToAction("Index", "login");
            }

        return View(view);

    }

}
