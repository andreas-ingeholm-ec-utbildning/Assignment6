using WebApp.Models.Entities;

namespace WebApp.Models;

public class Product
{

    public Guid ID { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public decimal Price { get; set; }

    public ProductCategoryEntity? Category { get; set; }
    public ICollection<ProductTagEntity> Tags { get; set; } = new HashSet<ProductTagEntity>();

}