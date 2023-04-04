using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class ProductRegisterView
{

    [Required(ErrorMessage = "You must assign a name to the product.")]
    [DisplayName("Product name *")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required(ErrorMessage = "You must assign a price for the product.")]
    [DisplayName("Price *")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

}
