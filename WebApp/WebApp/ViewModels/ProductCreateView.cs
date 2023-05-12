using WebApp.Models;
using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class ProductAddView
{
    public ProductAddForm Form { get; set; } = new();
    public IEnumerable<ProductCategoryEntity> Categories { get; set; } = null!;
}
