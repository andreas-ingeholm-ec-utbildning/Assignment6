using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers;

[Route("logout")]
public class UserLogoutController : Controller
{

    private readonly AuthService authService;

    public UserLogoutController(AuthService authService) =>
        this.authService = authService;

    public async Task<IActionResult> Index()
    {
        if (authService.IsSignedIn(User))
        {
            await authService.LogOutAsync();
            return RedirectToAction("index", "home");
        }
        else
            return RedirectToAction("index", "login");
    }

}
