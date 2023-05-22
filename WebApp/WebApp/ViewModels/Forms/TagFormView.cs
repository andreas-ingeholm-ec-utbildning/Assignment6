using System.ComponentModel.DataAnnotations;
using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class TagFormView
{

    public Guid ID { get; set; }

    [MinLength(3)]
    public string Name { get; set; } = null!;

    public static explicit operator TagFormView(TagEntity tag) =>
        new()
        {
            ID = tag.ID,
            Name = tag.Name,
        };

}