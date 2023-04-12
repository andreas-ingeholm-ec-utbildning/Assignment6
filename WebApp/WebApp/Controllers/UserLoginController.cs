using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Identity;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Route("login")]
public class UserLoginController : Controller
{

    private readonly SignInManager<User> signInManager;

    public UserLoginController(SignInManager<User> signInManager) =>
        this.signInManager = signInManager;

    [HttpGet]
    public IActionResult Index() =>
        View();

    [HttpPost]
    public async Task<IActionResult> Index(UserLoginView view)
    {

        if (ModelState.IsValid)
        {

            var result = await signInManager.PasswordSignInAsync(view.Email, view.Password, false, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                ModelState.AddModelError("", "Incorrect email address or password.");
        }

        return View(view);

    }

}
