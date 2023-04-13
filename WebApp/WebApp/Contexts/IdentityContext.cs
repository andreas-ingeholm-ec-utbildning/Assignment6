using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.Entities;
using WebApp.Models.Identity;

namespace WebApp.Contexts;

public class IdentityContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{

    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    { }

    public DbSet<UserProfileEntity> UserProfiles { get; set; }

}
