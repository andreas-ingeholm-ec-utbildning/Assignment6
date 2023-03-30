using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {

        ViewData["Title"] = "Homes";

        return View(new HomeIndexViewModel(
            new ShowcaseViewModel()
            {
                Ingress = "WELCOME TO BMEKETO SHOP",
                Title = "Exclusive chair gold collection",
                LinkContent = "SHOP NOW",
                LinkUrl = "/products",
                ImageUrl = "images/placeholders/625x647.svg",
            },
            new ShowcaseViewModel()
            {
                Ingress = "WELCOME TO BMEKETO SHOP",
                Title = "Exclusive chair gold collection 2",
                LinkContent = "SHOP NOW",
                LinkUrl = "/products",
                ImageUrl = "images/placeholders/625x647.svg",
            }
        ));

    }

}
