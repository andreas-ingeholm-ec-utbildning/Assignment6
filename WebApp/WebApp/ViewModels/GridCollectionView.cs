namespace WebApp.ViewModels;

public class GridCollectionView
{

    public string? Title { get; set; }
    public IEnumerable<string> Categories { get; set; } = null!;
    public IEnumerable<GridCollectionItemView> GridItems { get; set; } = null!;
    public bool LoadMore { get; set; } = false;

}