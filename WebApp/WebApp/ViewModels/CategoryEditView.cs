using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class CategoryEditView : CategoryAddView
{

    public Guid ID { get; set; }

    public static implicit operator CategoryEditView(ProductCategoryEntity category) =>
        new()
        {
            ID = category.ID,
            Name = category.Name,
        };

}
