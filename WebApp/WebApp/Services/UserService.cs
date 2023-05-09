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

    public UserService(IdentityContext identityContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        this.identityContext = identityContext;
        this.userManager = userManager;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserProfileEntity?> GetProfileEntityAsync(Guid userID) =>
        await identityContext.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserID == userID);

    public async Task<UserProfileEntity?> GetProfileEntityAsync(string email) =>
        await identityContext.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.User.Email == email);

    public async Task<IEnumerable<UserProfileEntity>> EnumerateProfiles() =>
        await identityContext.UserProfiles.Include(u => u.User).ToArrayAsync();

    public async Task<bool> Update(UserEditView view)
    {

        var isAuthenticated = httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated)
            return false;

        var email = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(email))
            return false;

        var existingUser = await GetProfileEntityAsync(email);
        if (existingUser is null)
            return false;

        existingUser.FirstName = view.FirstName ?? existingUser.FirstName;
        existingUser.LastName = view.LastName ?? existingUser.LastName;
        existingUser.StreetName = view.StreetName ?? existingUser.StreetName;
        existingUser.City = view.City ?? existingUser.City;
        existingUser.PostalCode = view.PostalCode ?? existingUser.PostalCode;

        //Update display name
        _ = await userManager.RemoveClaimsAsync(existingUser.User, (await userManager.GetClaimsAsync(existingUser.User)).Where(c => c.Type == "DisplayName"));
        _ = await userManager.AddClaimAsync(existingUser.User, new("DisplayName", $"{existingUser.FirstName} {existingUser.LastName}"));
        _ = await userManager.UpdateAsync(existingUser.User);

        _ = await identityContext.SaveChangesAsync();

        return true;

    }

    public async Task<bool> Update(UserEditAdminView view)
    {

        var existingUser = await GetProfileEntityAsync(view.ID);
        if (existingUser is null)
            return false;

        existingUser.FirstName = view.FirstName ?? existingUser.FirstName;
        existingUser.LastName = view.LastName ?? existingUser.LastName;
        existingUser.StreetName = view.StreetName ?? existingUser.StreetName;
        existingUser.City = view.City ?? existingUser.City;
        existingUser.PostalCode = view.PostalCode ?? existingUser.PostalCode;

        if (view.Role is UserRole.User or UserRole.Admin)
            await SetRoleAsync(existingUser.User, view.Role);

        //Update display name
        _ = await userManager.RemoveClaimsAsync(existingUser.User, (await userManager.GetClaimsAsync(existingUser.User)).Where(c => c.Type == "DisplayName"));
        _ = await userManager.AddClaimAsync(existingUser.User, new("DisplayName", $"{existingUser.FirstName} {existingUser.LastName}"));
        _ = await userManager.UpdateAsync(existingUser.User);

        _ = await identityContext.SaveChangesAsync();

        return true;

    }

    public async Task SetRoleAsync(User user, string role)
    {

        _ = await userManager.RemoveFromRoleAsync(user, UserRole.Admin);
        _ = await userManager.RemoveFromRoleAsync(user, UserRole.User);
        _ = await userManager.RemoveClaimAsync(user, (await userManager.GetClaimsAsync(user)).First(claim => claim.Type == ClaimTypes.Role));

        _ = await userManager.AddToRoleAsync(user, role);
        _ = await userManager.AddClaimAsync(user, new(ClaimTypes.Role, role));

    }

}