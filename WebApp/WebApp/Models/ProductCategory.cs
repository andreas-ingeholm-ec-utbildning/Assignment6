namespace WebApp.Models;

public class ProductCategory
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
}