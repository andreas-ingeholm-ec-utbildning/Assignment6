using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class ProductAddView
{

    [Required(ErrorMessage = "You must assign a name to the product.")]
    [DisplayName("Product name *")]
    public string Name { get; set; } = null!;

    [DataType(DataType.Upload)]
    public IFormFile? Image { get; set; }

    public string? ExistingImageUrl { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "You must assign a price for the product.")]
    [DisplayName("Price *")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    public Guid? Category { get; set; }

    public static implicit operator ProductEntity(ProductAddView view) =>
        new()
        {
            Name = view.Name,
            Description = view.Description,
            Price = view.Price,
            CategoryID = view.Category,
        };

}
