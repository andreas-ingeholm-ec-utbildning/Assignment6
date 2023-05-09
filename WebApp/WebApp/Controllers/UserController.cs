﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Identity;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class UserController : Controller
{

    readonly AuthService authService;
    readonly UserService userService;
    readonly SignInManager<User> signInManager;

    public UserController(AuthService authService, UserService userService, SignInManager<User> signInManager)
    {
        this.authService = authService;
        this.userService = userService;
        this.signInManager = signInManager;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {

        var user = await userService.GetProfileEntityAsync(User.Identity?.Name ?? string.Empty);
        if (user is null)
            return NotFound();

        return View((UserEditView)user);

    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Index(UserEditView view)
    {

        if (!ModelState.IsValid || !await userService.Update(view))
        {
            ModelState.AddModelError("", "Something went wrong when updating user.");
            return View(view);
        }
        else
        {
            //There seems to be no way to refresh cached claims? signInManager.RefreshSignInAsync() does not work.
            //Logging out seems to be the only reliable solution.
            await authService.LogOutAsync();
            return RedirectToAction("login");
        }

    }

    #region Login

    [HttpGet]
    public IActionResult Login() =>
        View();

    [HttpPost]
    public async Task<IActionResult> LogIn(UserLoginView view)
    {
        if (ModelState.IsValid)
            if (await authService.LoginAsync(view))
                return RedirectToAction("index", "home");
            else
                ModelState.AddModelError("", "Incorrect email address or password.");

        return View(view);
    }

    #endregion
    #region Logout

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        if (authService.IsSignedIn(User))
        {
            await authService.LogOutAsync();
            return RedirectToAction("index", "home");
        }
        else
            return RedirectToAction("login");
    }

    #endregion
    #region Register

    [HttpGet]
    public IActionResult Register() =>
        View(new UserRegisterView());

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterView view)
    {

        if (ModelState.IsValid)
            if (await authService.ExistsAsync(view.Email))
                ModelState.AddModelError("", "A user with the specified email address already exists.");
            else
            {
                if (await authService.RegisterAsync(view))
                    return RedirectToAction("login");
                else
                    ModelState.AddModelError("", "Something went wrong when creating user.");
            }

        return View(view);

    }

    #endregion

}
