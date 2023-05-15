namespace WebApp.Models.Entities;

public class ProductCategoryEntity
{

    public Guid ID { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

    public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();

    public static implicit operator ProductCategory(ProductCategoryEntity entity) =>
        new()
        {
            ID = entity.ID,
            Name = entity.Name,
        };

}
