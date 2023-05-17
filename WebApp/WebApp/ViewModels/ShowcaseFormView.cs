using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;
using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class ShowcaseFormView
{

    public Guid ID { get; set; }

    [MinLength(3)]
    [DisplayName("Ingress *")]
    public string Ingress { get; set; } = null!;

    [MinLength(3)]
    [DisplayName("Title *")]
    public string Title { get; set; } = null!;

    [MinLength(3)]
    [DisplayName("Link text *")]
    public string LinkText { get; set; } = null!;

    [MinLength(3)]
    [DisplayName("Link url *")]
    public string LinkUrl { get; set; } = null!;

    [DataType(DataType.Upload)]
    [DisplayName("Image *")]
    public virtual IFormFile? Image { get; set; }

    public string? CurrentImageUrl { get; set; }

    public static implicit operator ShowcaseEntity(ShowcaseFormView view) =>
        new()
        {
            ID = view.ID,
            Ingress = view.Ingress,
            Title = view.Title,
            LinkText = view.LinkText,
            LinkUrl = view.LinkUrl,
        };

    public static implicit operator ShowcaseFormView(Showcase showcase) =>
        new()
        {
            ID = showcase.ID,
            Title = showcase.Title,
            LinkText = showcase.LinkText,
            LinkUrl = showcase.LinkUrl,
            Ingress = showcase.Ingress,
            CurrentImageUrl = showcase.ImageUrl
        };

}
