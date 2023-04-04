using WebApp.Models;

namespace WebApp.Services;

public class ShowcaseService
{

    private readonly Showcase[] showcases = new Showcase[]
    {
        new()
        {
            Ingress = "WELCOME TO BMEKETO SHOP",
            Title = "Exclusive chair gold collection",
            ImageUrl = "images/placeholders/625x647.svg",
            LinkButton = new()
            {
                Content = "SHOP NOW",
                Url = "/products",
            }
        },
    };

    public bool ShowLatest { get; set; }

    public Showcase[] GetShowcases() =>
        showcases;

    public Showcase? GetLatest() =>
        showcases.LastOrDefault();

}
