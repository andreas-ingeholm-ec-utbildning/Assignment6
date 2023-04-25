using WebApp.Models;

namespace WebApp.ViewModels;

public class ProductAddView
{
    public ProductAddForm Form { get; set; } = new();
    public IEnumerable<ProductCategory> Categories { get; set; } = null!;
}
