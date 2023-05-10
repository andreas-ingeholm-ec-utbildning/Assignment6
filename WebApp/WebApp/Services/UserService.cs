using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;
using WebApp.Models.Entities;
using WebApp.Models.Identity;
using WebApp.ViewModels;

namespace WebApp.Services;

public class UserService
{

    readonly IdentityContext identityContext;
    readonly UserManager<User> userManager;
    readonly IHttpContextAccessor httpContextAccessor;
    readonly IServiceProvider serviceProvider;

    public UserService(IdentityContext identityContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
    {
        this.identityContext = identityContext;
        this.userManager = userManager;
        this.httpContextAccessor = httpContextAccessor;
        this.serviceProvider = serviceProvider;
    }

    public async Task<UserProfileEntity?> GetProfileEntityAsync(Guid userID) =>
        await identityContext.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserID == userID);

    public async Task<UserProfileEntity?> GetProfileEntityAsync(string email) =>
        await identityContext.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.User.Email == email);

    public UserProfileEntity? GetProfileEntity(string email) =>
        identityContext.UserProfiles.Include(u => u.User).FirstOrDefault(u => u.User.Email == email);

    public async Task<IEnumerable<UserProfileEntity>> EnumerateProfiles() =>
        await identityContext.UserProfiles.Include(u => u.User).ToArrayAsync();

    public async Task<bool> Update(UserEditView view)
    {

        if (GetLoggedInUser(out var userProfile))
        {

            await UpdateUserProfile(userProfile, view, true);
            _ = await identityContext.SaveChangesAsync();

            await RefreshClaims(userProfile.User);

            return true;

        }

        return false;

    }

    public async Task<bool> Update(UserEditAdminView view)
    {

        var userProfile = await GetProfileEntityAsync(view.ID);
        if (userProfile is null)
            return false;

        await UpdateUserProfile(userProfile, view, false);

        if (view.Role is UserRole.User or UserRole.Admin)
            await SetRoleAsync(userProfile.User, view.Role);

        await RefreshClaims(userProfile.User);

        return true;

    }

    async Task UpdateUserProfile(UserProfileEntity userProfile, UserEditView view, bool saveClaims)
    {

        userProfile.FirstName = view.FirstName ?? userProfile.FirstName;
        userProfile.LastName = view.LastName ?? userProfile.LastName;
        userProfile.StreetName = view.StreetName ?? userProfile.StreetName;
        userProfile.City = view.City ?? userProfile.City;
        userProfile.PostalCode = view.PostalCode ?? userProfile.PostalCode;

        //Update display name
        _ = await userManager.RemoveClaimsAsync(userProfile.User, (await userManager.GetClaimsAsync(userProfile.User)).Where(c => c.Type == "DisplayName"));
        _ = await userManager.AddClaimAsync(userProfile.User, new("DisplayName", $"{userProfile.FirstName} {userProfile.LastName}"));

        if (saveClaims)
            _ = await userManager.UpdateAsync(userProfile.User);

    }

    bool GetLoggedInEmail([NotNullWhen(true)] out string? email)
    {

        email = null;
        var isAuthenticated = httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated)
            return false;

        email = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        return !string.IsNullOrEmpty(email);

    }

    bool GetLoggedInUser([NotNullWhen(true)] out UserProfileEntity? userProfile)
    {

        userProfile = null;
        if (GetLoggedInEmail(out var email))
            userProfile = GetProfileEntity(email);

        return userProfile is not null;

    }

    public async Task SetRoleAsync(User user, string role)
    {

        _ = await userManager.RemoveFromRoleAsync(user, UserRole.Admin);
        _ = await userManager.RemoveFromRoleAsync(user, UserRole.User);
        _ = await userManager.RemoveClaimAsync(user, (await userManager.GetClaimsAsync(user)).First(claim => claim.Type == ClaimTypes.Role));

        _ = await userManager.AddToRoleAsync(user, role);
        _ = await userManager.AddClaimAsync(user, new(ClaimTypes.Role, role));
        _ = await userManager.UpdateAsync(user);

    }

    async Task RefreshClaims(User user)
    {
        var signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
        await signInManager.RefreshSignInAsync(user);
    }

}