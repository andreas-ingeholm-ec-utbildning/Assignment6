using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Route("account/login")]
public class UserLoginController : Controller
{

    private readonly AuthService authService;

    public UserLoginController(AuthService authService) =>
        this.authService = authService;

    [HttpGet]
    public IActionResult Index() =>
        View();

    [HttpPost]
    public async Task<IActionResult> Index(UserLoginView view)
    {

        if (ModelState.IsValid)
            if (await authService.LoginAsync(view))
                return RedirectToAction("Index", "Home");
            else
                ModelState.AddModelError("", "Incorrect email address or password.");

        return View(view);

    }

}
