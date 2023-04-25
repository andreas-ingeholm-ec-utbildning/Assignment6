using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("user")]
[Authorize]
public class UserAccountController : Controller
{

    public IActionResult Index() =>
        View();

}
