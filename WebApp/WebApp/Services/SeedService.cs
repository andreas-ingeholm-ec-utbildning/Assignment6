using Microsoft.AspNetCore.Identity;

namespace WebApp.Services;

/// <summary>Sets up roles.</summary>
public class SeedService
{

    readonly RoleManager<IdentityRole<Guid>> roleManager;

    public SeedService(RoleManager<IdentityRole<Guid>> roleManager) =>
        this.roleManager = roleManager;

    public async Task SeedRoles()
    {

        await EnsureExists(UserRole.Admin);
        await EnsureExists(UserRole.User);

        async Task EnsureExists(string name)
        {
            if (!await roleManager.RoleExistsAsync(name))
                _ = await roleManager.CreateAsync(new(name));
        }

    }

}
