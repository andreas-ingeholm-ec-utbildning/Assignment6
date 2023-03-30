namespace WebApp.ViewModels;

public class HomeIndexViewModel
{

    public HomeIndexViewModel(params ShowcaseViewModel[] showcases) =>
        Showcases = showcases;

    public ShowcaseViewModel[] Showcases { get; set; }

}
