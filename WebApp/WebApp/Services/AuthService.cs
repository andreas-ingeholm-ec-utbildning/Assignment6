using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;
using WebApp.Models.Entities;
using WebApp.Models.Identity;
using WebApp.ViewModels;

namespace WebApp.Services;

public class AuthService
{

    readonly UserManager<User> userManager;
    readonly SignInManager<User> signInManager;
    readonly IdentityContext identityContext;
    readonly SeedService seedService;
    readonly RoleManager<IdentityRole<Guid>> roleManager;

    public AuthService(UserManager<User> userManager, IdentityContext identityContext, SignInManager<User> signInManager, SeedService seedService, RoleManager<IdentityRole<Guid>> roleManager)
    {
        this.userManager = userManager;
        this.identityContext = identityContext;
        this.signInManager = signInManager;
        this.seedService = seedService;
        this.roleManager = roleManager;
    }

    public async Task<bool> ExistsAsync(string emailAddress) =>
        await userManager.FindByEmailAsync(emailAddress) is not null;

    public async Task<bool> RegisterAsync(UserRegisterView view)
    {

        try
        {

            //Ensure roles are created
            await seedService.SeedRoles();

            //First user will be admin, otherwise user
            var role = !await userManager.Users.AnyAsync()
                ? UserRole.Admin
                : UserRole.User;

            var user = (User)view;
            _ = await userManager.CreateAsync(user, view.Password);

            _ = await userManager.AddToRoleAsync(user, role);

            var profile = (UserProfileEntity)view;
            profile.UserID = user.Id;
            _ = await identityContext.UserProfiles.AddAsync(profile);
            _ = await identityContext.SaveChangesAsync();

            return true;

        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LoginAsync(UserLoginView view)
    {

        try
        {
            var result = await signInManager.PasswordSignInAsync(view.Email, view.Password, view.RememberMe, false);
            return result.Succeeded;
        }
        catch
        {
            return false;
        }

    }

    public bool IsSignedIn(ClaimsPrincipal user) =>
        signInManager.IsSignedIn(user);

    public Task LogOutAsync() =>
        signInManager.SignOutAsync();

}