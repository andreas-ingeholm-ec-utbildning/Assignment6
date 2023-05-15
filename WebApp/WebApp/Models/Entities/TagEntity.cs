namespace WebApp.Models.Entities;

public class TagEntity
{

    public Guid ID { get; set; }
    public string Name { get; set; } = null!;

    public static implicit operator Tag(TagEntity entity) =>
        new()
        {
            ID = entity.ID,
            Name = entity.Name,
        };

}
