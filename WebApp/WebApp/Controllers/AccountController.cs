using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AccountController : Controller
{

    private readonly UserService userService;

    public AccountController(UserService userService) =>
        this.userService = userService;

    public IActionResult Index() => View();

    #region Login

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginView data)
    {

        if (ModelState.IsValid)
        {

            if (await userService.LoginAsync(data))
                return RedirectToAction("Index", "Account");
            else
                ModelState.AddModelError("", "Invalid email address or password.");

        }

        return View(data);

    }

    #endregion
    #region Register

    public IActionResult Register() =>
        View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterView data)
    {

        if (ModelState.IsValid)
        {

            if (await userService.UserExists(u => u.EmailAddress == data.EmailAddress))
                ModelState.AddModelError("", "An account with the specified email address already exists.");
            else if (!await userService.RegisterAsync(data))
                ModelState.AddModelError("", "Something went wrong when creating user, please try again.");
            else
                return RedirectToAction("Home", "Index");

        }

        return View(data);

    }

    #endregion

}
