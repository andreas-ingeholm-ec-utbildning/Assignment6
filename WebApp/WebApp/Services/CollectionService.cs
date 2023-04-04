using WebApp.Models;

namespace WebApp.Services;

public class CollectionService
{

    //Used for testing
    int count;

    string GetID() =>
        (count += 1).ToString();

    public IEnumerable<Collection> GetCollections() =>
        new Collection[] {
            new()
            {
                Title = "Best Collection",
                Categories = new[] { "All", "Bags", "Dresses", "Decorations", "Essentials", "Interior", "Laptops", "Mobile", "Beauty" },
                Items = new CollectionItem[]
                {
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                }
            },
            new()
            {
                Title = "Up to Sale",
                Categories = new[] { "All", "Bags", "Dresses", "Decorations", "Essentials", "Interior", "Laptops", "Mobile", "Beauty" },
                Items = new CollectionItem[]
                {
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                }
            },
            new()
            {
                Title = "Best selling",
                Categories = new[] { "All", "Bags", "Dresses", "Decorations", "Essentials", "Interior", "Laptops", "Mobile", "Beauty" },
                Items = new CollectionItem[]
                {
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                    new() { Id = GetID(), Title = "Apple watch collection " + count, Price = 30, ImageUrl = "images/placeholders/270x295.svg" },
                }
            },
        };

}