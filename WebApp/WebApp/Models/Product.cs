namespace WebApp.Models;

public class Product
{

    public Guid ID { get; set; }
    public bool IsVisible { get; set; } = true;

    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }

    public ProductCategory? Category { get; set; }
    public List<Tag> Tags { get; set; } = new();

}
