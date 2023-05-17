using WebApp.Models;

namespace WebApp.ViewModels;

public class SearchView
{
    public string Query { get; set; } = string.Empty;
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
}