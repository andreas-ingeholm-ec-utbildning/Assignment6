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

    public async Task<UserProfileEntity?> GetAsync(Guid userID) =>
        await identityContext.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserID == userID);

    public async Task<UserProfileEntity?> GetAsync(string email) =>
        await identityContext.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.User.Email == email);

    public UserProfileEntity? Get(string email) =>
        identityContext.UserProfiles.Include(u => u.User).FirstOrDefault(u => u.User.Email == email);

    public async Task<IEnumerable<UserProfileEntity>> EnumerateAsync() =>
        await identityContext.UserProfiles.Include(u => u.User).ToArrayAsync();

    public async Task<bool> UpdateAsync(UserEditView view)
    {

        if (GetLoggedInUser(out var userProfile))
        {

            await UpdateUserProfileNoSave(userProfile, view);
            _ = await userManager.UpdateAsync(userProfile.User);
            _ = await identityContext.SaveChangesAsync();
            await RefreshClaims(userProfile.User);

            return true;

        }

        return false;

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
            userProfile = Get(email);

        return userProfile is not null;

    }

    public async Task<bool> UpdateAsync(UserEditAdminView view)
    {

        var userProfile = await GetAsync(view.ID);
        if (userProfile is null)
            return false;

        await UpdateUserProfileNoSave(userProfile, view);

        if (view.Role is UserRole.User or UserRole.Admin)
            await SetRoleAsync(userProfile.User, view.Role);

        _ = await userManager.UpdateAsync(userProfile.User);

        _ = await identityContext.SaveChangesAsync();
        await RefreshClaims(userProfile.User);

        return true;

    }

    async Task UpdateUserProfileNoSave(UserProfileEntity userProfile, UserEditView view)
    {

        //Update info
        userProfile.FirstName = view.FirstName ?? userProfile.FirstName;
        userProfile.LastName = view.LastName ?? userProfile.LastName;
        userProfile.StreetName = view.StreetName ?? userProfile.StreetName;
        userProfile.City = view.City ?? userProfile.City;
        userProfile.PostalCode = view.PostalCode ?? userProfile.PostalCode;

        //Update DisplayName claim
        _ = await userManager.RemoveClaimsAsync(userProfile.User, await GetClaimsAsync(userProfile.User, UserClaim.DisplayName));
        _ = await userManager.AddClaimAsync(userProfile.User, new(UserClaim.DisplayName, $"{userProfile.FirstName} {userProfile.LastName}"));

    }

    async Task<IEnumerable<Claim>> GetClaimsAsync(User user, string claimType) =>
       (await userManager.GetClaimsAsync(user)).Where(c => c.Type == claimType);

    async Task SetRoleAsync(User user, string role)
    {

        //Clear current role and claim
        _ = await userManager.RemoveFromRoleAsync(user, UserRole.Admin);
        _ = await userManager.RemoveFromRoleAsync(user, UserRole.User);
        _ = await userManager.RemoveClaimAsync(user, (await userManager.GetClaimsAsync(user)).First(claim => claim.Type == ClaimTypes.Role));

        //Add new role and claim
        _ = await userManager.AddToRoleAsync(user, role);
        _ = await userManager.AddClaimAsync(user, new(ClaimTypes.Role, role));

    }

    async Task RefreshClaims(User user)
    {
        var signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
        await signInManager.RefreshSignInAsync(user);
    }

}