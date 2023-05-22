using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;
using WebApp.Models.Entities;
using WebApp.Models.Identity;
using WebApp.ViewModels;

namespace WebApp.Services;

/// <summary>Manages users.</summary>
public class UserService
{

    #region Injections

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

    #endregion
    #region Get

    /// <summary>Gets the user with the specified <paramref name="userID"/>, if one exists.</summary>
    public async Task<UserProfileEntity?> GetAsync(Guid userID) =>
        await identityContext.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserID == userID);

    /// <summary>Gets the user with the specified <paramref name="email"/>, if one exists.</summary>
    public async Task<UserProfileEntity?> GetAsync(string email) =>
        await identityContext.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.User.Email == email);

    /// <inheritdoc cref="GetAsync(string)"/>
    public UserProfileEntity? Get(string email) =>
        identityContext.UserProfiles.Include(u => u.User).FirstOrDefault(u => u.User.Email == email);

    /// <summary>Enumerates all users.</summary>
    public async Task<IEnumerable<UserProfileEntity>> EnumerateAsync() =>
        await identityContext.UserProfiles.Include(u => u.User).ToArrayAsync();

    /// <summary>Gets the email of the logged in user.</summary>
    bool GetLoggedInEmail([NotNullWhen(true)] out string? email)
    {

        email = null;
        var isAuthenticated = httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated)
            return false;

        email = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        return !string.IsNullOrEmpty(email);

    }

    /// <summary>Gets the profile of the logged in user.</summary>
    bool GetLoggedInUser([NotNullWhen(true)] out UserProfileEntity? userProfile)
    {

        userProfile = null;
        if (GetLoggedInEmail(out var email))
            userProfile = Get(email);

        return userProfile is not null;

    }

    #endregion
    #region Update

    /// <summary>Updates a user.</summary>
    public async Task<bool> UpdateAsync(UserEditView view)
    {

        if (GetLoggedInUser(out var userProfile))
        {

            await UpdateUserProfileInfo(userProfile, view);
            _ = await userManager.UpdateAsync(userProfile.User);
            _ = await identityContext.SaveChangesAsync();
            await RefreshClaims(userProfile.User);

            return true;

        }

        return false;

    }

    /// <summary>Updates a user.</summary>
    public async Task<bool> UpdateAsync(UserFormAdminView view)
    {

        var userProfile = await GetAsync(view.ID);
        if (userProfile is null)
            return false;

        await UpdateUserProfileInfo(userProfile, view);

        if (view.Role is UserRole.User or UserRole.Admin)
            await SetRoleAsync(userProfile.User, view.Role);

        _ = await userManager.UpdateAsync(userProfile.User);

        _ = await identityContext.SaveChangesAsync();
        await RefreshClaims(userProfile.User);

        return true;

    }

    /// <summary>Updates user profile info, and DisplayName claim.</summary>
    /// <remarks>Does not save to db.</remarks>
    async Task UpdateUserProfileInfo(UserProfileEntity userProfile, UserEditView view)
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

    /// <summary>Gets the claims of a user.</summary>
    async Task<IEnumerable<Claim>> GetClaimsAsync(User user, string claimType) =>
       (await userManager.GetClaimsAsync(user)).Where(c => c.Type == claimType);

    /// <summary>Sets the role of a user.</summary>
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

    /// <summary>Refreshes claims for the specified <paramref name="user"/>.</summary>
    async Task RefreshClaims(User user)
    {
        var signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
        await signInManager.RefreshSignInAsync(user);
    }

    #endregion
    #region Delete

    public async Task<bool> Delete(Guid id)
    {

        if (GetLoggedInUser(out var loggedInUser) && loggedInUser.UserID == id)
            return false;

        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
            return false;

        await userManager.DeleteAsync(user);
        return true;

    }

    #endregion

}
