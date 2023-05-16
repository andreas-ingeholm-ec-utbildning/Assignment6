using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;
using WebApp.Models.Entities;
using WebApp.Models.Identity;
using WebApp.ViewModels;

namespace WebApp.Services;

/// <summary>Manages authorization.</summary>
public class AuthService
{

    readonly UserManager<User> userManager;
    readonly SignInManager<User> signInManager;
    readonly IdentityContext identityContext;
    readonly SeedService seedService;

    public AuthService(UserManager<User> userManager, IdentityContext identityContext, SignInManager<User> signInManager, SeedService seedService)
    {
        this.userManager = userManager;
        this.identityContext = identityContext;
        this.signInManager = signInManager;
        this.seedService = seedService;
    }

    /// <summary>Gets if a user exist with the specified <paramref name="emailAddress"/>.</summary>
    public async Task<bool> ExistsAsync(string emailAddress) =>
        await userManager.FindByEmailAsync(emailAddress) is not null;

    /// <summary>Gets the user with the specified <paramref name="emailAddress"/>, if one exists.</summary>
    public async Task<User?> FindByEmailAsync(string emailAddress) =>
        await userManager.FindByEmailAsync(emailAddress);

    /// <summary>Registers a user.</summary>
    public async Task<bool> RegisterAsync(UserRegisterView view)
    {

        try
        {

            //Ensure roles are created
            await seedService.SeedRoles();

            //First user will be admin, otherwise user
            var role =
                !await userManager.Users.AnyAsync()
                ? UserRole.Admin
                : UserRole.User;

            var user = (User)view;
            _ = await userManager.CreateAsync(user, view.Password);

            _ = await userManager.AddToRoleAsync(user, role);
            _ = await userManager.AddClaimAsync(user, new(ClaimTypes.Role, role));

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

    /// <summary>Gets the role of a user.</summary>
    public async Task<string> GetRoleAsync(User user) =>
        (await userManager.GetRolesAsync(user)).First();

    /// <summary>Logs a user in.</summary>
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

    /// <summary>Gets if we have a user signed-in.</summary>
    public bool IsSignedIn(ClaimsPrincipal user) =>
        signInManager.IsSignedIn(user);

    /// <summary>Logs the current user out.</summary>
    public Task LogOutAsync() =>
        signInManager.SignOutAsync();

}