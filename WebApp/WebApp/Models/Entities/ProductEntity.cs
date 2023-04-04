using System.ComponentModel.DataAnnotations.Schema;
using WebApp.ViewModels;

namespace WebApp.Models.Entities;

public class ProductEntity
{

    public Guid ID { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public static implicit operator ProductEntity(ProductRegisterView data) =>
        new()
        {
            Name = data.Name,
            Description = data.Description,
            Price = data.Price,
        };

    public static implicit operator Product?(ProductEntity? entity) =>
        entity is null
        ? null
        : new()
        {
            ID = entity.ID,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price
        };

}