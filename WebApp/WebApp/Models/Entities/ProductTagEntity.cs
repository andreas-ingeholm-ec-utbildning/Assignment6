using Microsoft.EntityFrameworkCore;

namespace WebApp.Models.Entities;

[PrimaryKey(nameof(ID), nameof(TagId))]
public class ProductTagEntity
{

    public Guid ID { get; set; } = Guid.NewGuid();
    public ProductEntity Product { get; set; } = null!;

    public Guid TagId { get; set; }
    public TagEntity Tag { get; set; } = null!;

}
