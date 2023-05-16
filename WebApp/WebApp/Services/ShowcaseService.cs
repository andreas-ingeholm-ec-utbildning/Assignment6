using WebApp.Models;

namespace WebApp.Services;

/// <summary>Manages showcases.</summary>
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

    /// <summary>Gets all showcases.</summary>
    public Showcase[] GetShowcases() =>
        showcases;

    /// <summary>Get latest.</summary>
    public Showcase? GetLatest() =>
        showcases.LastOrDefault();

}
