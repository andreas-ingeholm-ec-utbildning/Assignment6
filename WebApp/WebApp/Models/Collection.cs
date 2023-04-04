namespace WebApp.Models;

public class Collection
{

    public string? Title { get; set; }
    public IEnumerable<string> Categories { get; set; } = null!;
    public IEnumerable<CollectionItem> Items { get; set; } = null!;
    public int LoadMoreThreshold { get; set; } = 8;

    public bool CanLoadMore => Items.Count() > LoadMoreThreshold;

}
