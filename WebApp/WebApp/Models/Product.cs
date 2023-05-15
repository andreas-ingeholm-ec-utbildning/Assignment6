using WebApp.Models.Entities;

namespace WebApp.Models;

public class Product
{

    public Guid ID { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }

    public ProductCategoryEntity? Category { get; set; }

    public List<ProductTagEntity> Tags { get; set; } = new();

}
