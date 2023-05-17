using System.ComponentModel.DataAnnotations;
using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class CategoryFormView
{

    public Guid ID { get; set; }

    [MinLength(3)]
    public string Name { get; set; } = null!;

    public static implicit operator CategoryFormView(ProductCategoryEntity category) =>
        new()
        {
            ID = category.ID,
            Name = category.Name,
        };

}
