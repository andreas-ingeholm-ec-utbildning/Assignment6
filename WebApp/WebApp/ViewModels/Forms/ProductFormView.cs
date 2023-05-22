using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;
using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class ProductFormView
{

    public Guid ID { get; set; }

    [DisplayName("Is visible")]
    public bool IsVisible { get; set; }

    [Required(ErrorMessage = "You must assign a name to the product.")]
    [DisplayName("Product name *")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required(ErrorMessage = "You must assign a price for the product.")]
    [DisplayName("Price *")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [DataType(DataType.Upload)]
    [DisplayName("Image")]
    public virtual IFormFile? Image { get; set; }

    public string? CurrentImageUrl { get; set; }

    public Guid? Category { get; set; }

    public static implicit operator ProductEntity(ProductFormView view) =>
        new()
        {
            ID = view.ID,
            IsVisible = view.IsVisible,
            Name = view.Name,
            Description = view.Description,
            Price = view.Price,
            CategoryID = view.Category,
        };

    public static implicit operator ProductFormView(Product product) =>
        new()
        {
            ID = product.ID,
            IsVisible = product.IsVisible,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category?.ID,
            CurrentImageUrl = product.ImageUrl,
        };

}
