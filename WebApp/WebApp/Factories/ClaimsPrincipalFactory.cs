using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApp.Models.Identity;
using WebApp.Services;

namespace WebApp.Factories;

public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole<Guid>>
{

    readonly UserService userService;

    public ClaimsPrincipalFactory(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, IOptions<IdentityOptions> optionsAccessor, UserService userService) :
        base(userManager, roleManager, optionsAccessor) =>
        this.userService = userService;

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {

        var claimsIdentity = await base.GenerateClaimsAsync(user);

        var profile = await userService.GetAsync(user.Id);
        if (profile is not null)
            claimsIdentity.AddClaim(new(UserClaim.DisplayName, $"{profile.FirstName} {profile.LastName}"));

        return claimsIdentity;

    }

}
