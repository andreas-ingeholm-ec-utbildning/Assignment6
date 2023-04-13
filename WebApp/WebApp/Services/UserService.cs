using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;
using WebApp.Models.Entities;

namespace WebApp.Services;

public class UserService
{

    readonly IdentityContext identityContext;

    public UserService(IdentityContext identityContext) =>
        this.identityContext = identityContext;

    public async Task<UserProfileEntity?> GetProfileEntityAsync(Guid userID) =>
        await identityContext.UserProfiles.Include(u => u.User).FirstOrDefaultAsync(u => u.UserID == userID);

}