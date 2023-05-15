using WebApp.Models;

namespace WebApp.ViewModels;

public class ProductEditView : ProductAddView
{

    public Guid ID { get; set; }

    public static implicit operator ProductEditView(Product product) =>
        new()
        {
            ID = product.ID,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category?.ID,
            ExistingImageUrl = product.ImageUrl
        };

}
