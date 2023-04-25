﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApp.Models.Identity;
using WebApp.Services;

namespace WebApp.Factories;

public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{

    readonly UserService userService;

    public ClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor, UserService userService) : base(userManager, optionsAccessor) =>
        this.userService = userService;

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {

        var claimsIdentity = await base.GenerateClaimsAsync(user);

        var profile = await userService.GetProfileEntityAsync(user.Id);
        if (profile is not null)
            claimsIdentity.AddClaim(new("DisplayName", $"{profile.FirstName} {profile.LastName}"));

        return claimsIdentity;

    }

}