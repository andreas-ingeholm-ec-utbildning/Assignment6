using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Entities;

public class ProductEntity
{

    [Key]
    public Guid ID { get; set; } = Guid.NewGuid();
    public bool IsVisible { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public Guid? CategoryID { get; set; }
    public ProductCategoryEntity? Category { get; set; }

    public List<ProductTagEntity> Tags { get; set; } = new();

    public static implicit operator Product(ProductEntity entity) =>
        new()
        {
            ID = entity.ID,
            IsVisible = entity.IsVisible,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Category = (ProductCategory?)entity.Category!,
            ImageUrl = entity.ImageUrl,
            Tags = entity.Tags.Select(t => (Tag)t).ToList()
        };

}
