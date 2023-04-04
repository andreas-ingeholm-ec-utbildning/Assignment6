using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels;

public class ProductEditView
{

    public Guid? ID { get; set; } = null!;

    [DisplayName("Product name")]
    public string? Name { get; set; } = null!;

    public string? Description { get; set; }

    [DisplayName("Price")]
    [DataType(DataType.Currency)]
    public decimal? Price { get; set; }

    public static implicit operator ProductEditView(Product product) =>
        new()
        {
            ID = product.ID,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
        };

}
