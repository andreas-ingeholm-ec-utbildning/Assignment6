using WebApp.Models.Entities;

namespace WebApp.ViewModels;

public class TagEditView : TagAddView
{

    public Guid ID { get; set; }

    public static implicit operator TagEditView(TagEntity tag) =>
        new()
        {
            ID = tag.ID,
            Name = tag.Name,
        };

}