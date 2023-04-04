using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;
using WebApp.Models.Entities;
using WebApp.ViewModels;

namespace WebApp.Services;

public class UserService
{

    private readonly DataContext context;

    public UserService(DataContext context) =>
        this.context = context;

    public Task<bool> UserExists(Expression<Func<UserEntity, bool>> predicate) =>
        context.Users.AnyAsync(predicate);

    public Task<UserEntity?> Find(Expression<Func<UserEntity, bool>> predicate) =>
        context.Users.FirstOrDefaultAsync(predicate);

    public Task<UserEntity?> Find(string emailAddress) =>
        Find(u => u.EmailAddress == emailAddress);

    public Task<UserEntity?> Find(Guid id) =>
        Find(u => u.ID == id);

    public async Task<bool> RegisterAsync(RegisterView data)
    {

        try
        {

            //Add UserEntity + UserProfileEntity to db

            var userEntity = (UserEntity)data;
            var profileEntity = (ProfileEntity)data;

            _ = await context.Users.AddAsync(userEntity);
            _ = await context.SaveChangesAsync();

            profileEntity.UserID = userEntity.ID;
            _ = await context.Profiles.AddAsync(profileEntity);
            _ = await context.SaveChangesAsync();

            return true;

        }
        catch (Exception)
        {
            return false;
        }

    }

    public async Task<bool> LoginAsync(LoginView data) =>
        (await Find(data.EmailAddress))?.ValidatePassword(data.Password) ?? false;

}
