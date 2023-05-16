using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class ProductsController : Controller
{

    public IActionResult Index() =>
        View();

    public IActionResult Search() =>
        View();

}
