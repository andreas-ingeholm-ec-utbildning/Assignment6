namespace WebApp.Models;

public class ProductCollection
{

    public string Header { get; set; } = null!;
    public IEnumerable<Product> Products { get; set; } = null!;
    public IEnumerable<ProductCategory> Categories { get; set; } = null!;

    public static implicit operator ProductCollection(KeyValuePair<Tag, List<Product>> kvp) =>
        new()
        {
            Header = kvp.Key.Name,
            Products = kvp.Value,
            Categories = kvp.Value.Select(p => p.Category).OfType<ProductCategory>()
        };

}
