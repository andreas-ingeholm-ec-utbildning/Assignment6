namespace WebApp.Models;

public class Showcase
{

    public Guid ID { get; set; }
    public bool IsVisible { get; set; } = true;

    public string Ingress { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? ImageUrl { get; set; } = null!;
    public string LinkText { get; set; } = null!;
    public string LinkUrl { get; set; } = null!;

}
