using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Models.Entities;

namespace WebApp.Models;

public class ProductAddForm
{

    [Required(ErrorMessage = "You must assign a name to the product.")]
    [DisplayName("Product name *")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required(ErrorMessage = "You must assign a price for the product.")]
    [DisplayName("Price *")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    public ProductCategoryEntity Category { get; set; } = null!;

    public static implicit operator ProductEntity(ProductAddForm form) =>
        new()
        {
            Name = form.Name,
            Description = form.Description,
            Price = form.Price,
            CategoryId = form.Category.ID
        };

}
