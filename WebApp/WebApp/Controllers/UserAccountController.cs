using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers;

[Route("user")]
public class UserAccountController : Controller
{

    readonly AuthService authService;

    public UserAccountController(AuthService authService) =>
        this.authService = authService;

    [Authorize]
    public IActionResult Index() =>
        View();

}
