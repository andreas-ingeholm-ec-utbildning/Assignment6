using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;
using WebApp.Factories;
using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Models.Identity;
using WebApp.Repositories;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("products")));
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("identity")));
builder.Services.AddDbContext<ContactContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("contact")));

builder.Services.AddScoped<Repo<ProductEntity>>();
builder.Services.AddScoped<Repo<ProductCategoryEntity>>();
builder.Services.AddScoped<Repo<TagEntity>>();
builder.Services.AddScoped<Repo<ProductTagEntity>>();
builder.Services.AddScoped<Repo<ShowcaseEntity>>();
builder.Services.AddScoped<Repo<ContactData, ContactContext>>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SeedService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ShowcaseService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<ShowcaseService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<ContactService>();

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
}).
AddRoles<IdentityRole<Guid>>().
AddEntityFrameworkStores<IdentityContext>().
AddClaimsPrincipalFactory<ClaimsPrincipalFactory>();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, ClaimsPrincipalFactory>();

builder.Services.AddAuthorization(options =>
    options.AddPolicy(UserRole.Admin, policy => policy.RequireRole(UserRole.Admin)));

var app = builder.Build();
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
