using System.Security.Claims;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Web.Models.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AaramEducation.Web.Controllers;

public class AccountController(IUserRepository users, IWebHostEnvironment env) : Controller
{
    [HttpGet]
    public IActionResult Register() =>
        User.Identity?.IsAuthenticated == true ? RedirectToAction("Index", "Home") : View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        if (await users.EmailExistsAsync(vm.Email))
        {
            ModelState.AddModelError("Email", "An account with this email already exists.");
            return View(vm);
        }
        if (vm.Role == UserRole.Admin)
        {
            ModelState.AddModelError("Role", "Invalid role selection.");
            return View(vm);
        }

        var user = new User
        {
            Email = vm.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.Password),
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            Role = vm.Role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        await users.CreateAsync(user);
        await SignInUserAsync(user, false);
        return RedirectToAction("Index", "Courses");
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Home");
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = await users.GetByEmailAsync(vm.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(vm.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(vm);
        }

        await SignInUserAsync(user, vm.RememberMe);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Courses");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [Authorize, HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await GetCurrentUserAsync();
        if (user is null) return RedirectToAction("Login");
        return View(new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ProfilePicture = user.ProfilePicture,
        });
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = await GetCurrentUserAsync();
        if (user is null) return RedirectToAction("Login");

        if (!user.Email.Equals(vm.Email, StringComparison.OrdinalIgnoreCase)
            && await users.EmailExistsAsync(vm.Email))
        {
            ModelState.AddModelError("Email", "That email is already in use.");
            return View(vm);
        }

        user.FirstName = vm.FirstName;
        user.LastName = vm.LastName;
        user.Email = vm.Email;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(vm.NewPassword))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.NewPassword);

        if (vm.NewProfilePicture is { Length: > 0 })
        {
            var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowed.Contains(vm.NewProfilePicture.ContentType))
            {
                ModelState.AddModelError("NewProfilePicture", "Only JPEG, PNG, or WebP images allowed.");
                return View(vm);
            }
            var ext = Path.GetExtension(vm.NewProfilePicture.FileName);
            var fileName = $"{user.UserId}{ext}";
            var path = Path.Combine(env.WebRootPath, "uploads", "avatars", fileName);
            await using var stream = new FileStream(path, FileMode.Create);
            await vm.NewProfilePicture.CopyToAsync(stream);
            user.ProfilePicture = $"/uploads/avatars/{fileName}";
        }

        await users.UpdateAsync(user);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await SignInUserAsync(user, false);

        TempData["Success"] = "Profile updated successfully.";
        return RedirectToAction("Profile");
    }

    private async Task SignInUserAsync(User user, bool isPersistent)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Role, user.Role.ToString()),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = isPersistent });
    }

    private async Task<User?> GetCurrentUserAsync()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idStr, out var id) ? await users.GetByIdAsync(id) : null;
    }
}
