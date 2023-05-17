using Microsoft.EntityFrameworkCore;

namespace WebApp.Models.Entities;

[PrimaryKey(nameof(ProductID), nameof(TagID))]
public class ProductTagEntity
{

    public Guid ProductID { get; set; }
    public ProductEntity Product { get; set; } = null!;

    public Guid TagID { get; set; }
    public TagEntity Tag { get; set; } = null!;

    public static implicit operator Tag(ProductTagEntity entity) =>
        new()
        {
            ID = entity.TagID,
            Name = entity.Tag.Name
        };

}
