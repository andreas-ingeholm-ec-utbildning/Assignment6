namespace WebApp.Models.Entities;

public class ShowcaseEntity
{

    public Guid ID { get; set; }
    public bool IsVisible { get; set; }

    public string Ingress { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? ImageUrl { get; set; } = null!;
    public string LinkText { get; set; } = null!;
    public string LinkUrl { get; set; } = null!;

    public static implicit operator Showcase(ShowcaseEntity entity) =>
        new()
        {
            ID = entity.ID,
            IsVisible = entity.IsVisible,
            Ingress = entity.Ingress,
            Title = entity.Title,
            ImageUrl = entity.ImageUrl,
            LinkText = entity.LinkText,
            LinkUrl = entity.LinkUrl,
        };

}