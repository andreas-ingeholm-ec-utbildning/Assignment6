using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Route("register")]
public class UserRegisterController : Controller
{

    private readonly AuthService authService;

    public UserRegisterController(AuthService authService) =>
        this.authService = authService;

    [HttpGet]
    public IActionResult Index() =>
        View(new UserRegisterView());

    [HttpPost]
    public async Task<IActionResult> Index(UserRegisterView view)
    {

        if (ModelState.IsValid)
            if (await authService.ExistsAsync(view.Email))
                ModelState.AddModelError("", "A user with the specified email address already exists.");
            else
            {
                if (await authService.RegisterAsync(view))
                    return RedirectToAction("Index", "login");
                else
                    ModelState.AddModelError("", "Something went wrong when creating user.");
            }

        return View(view);

    }

}
