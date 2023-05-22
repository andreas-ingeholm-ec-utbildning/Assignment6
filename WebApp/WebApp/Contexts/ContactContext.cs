using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Contexts;

public class ContactContext : DbContext
{

    public ContactContext(DbContextOptions options) : base(options)
    { }

    public DbSet<ContactData> ContactData { get; set; }

}