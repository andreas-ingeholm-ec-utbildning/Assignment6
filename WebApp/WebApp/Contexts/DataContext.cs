using Microsoft.EntityFrameworkCore;
using WebApp.Models.Entities;

namespace WebApp.Contexts;

public class DataContext : DbContext
{

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }

    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductCategoryEntity> Categories { get; set; }
    public DbSet<ProductTagEntity> ProductTags { get; set; }
    public DbSet<TagEntity> Tags { get; set; }

}
