using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Identity;

namespace WebApp.Controllers;

[Route("logout")]
public class UserLogoutController : Controller
{

    private readonly SignInManager<User> signInManager;

    public UserLogoutController(SignInManager<User> signInManager) =>
        this.signInManager = signInManager;

    public async Task<IActionResult> Index()
    {
        if (signInManager.IsSignedIn(User))
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        else
            return RedirectToAction("index", "login");
    }

}
