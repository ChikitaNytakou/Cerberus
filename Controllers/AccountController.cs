using ByeBye.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByeBye.Controllers
{
    public class AccountController : Controller
    {
        //userManager will hold the UserManager instance
        private readonly UserManager<IdentityUser> userManager;

        //signInManager will hold the SignInManager instance
        private readonly SignInManager<IdentityUser> signInManager;

        //Both UserManager and SignInManager services are injected into the AccountController
        //using constructor injection
        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.Users.SingleOrDefaultAsync(u => u.UserName == model.Login);

                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Handle successful login

                        // Check if the ReturnUrl is not null and is a local URL
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            // Redirect to default page
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    if (result.RequiresTwoFactor)
                    {
                        // Handle two-factor authentication case
                    }
                    if (result.IsLockedOut)
                    {
                        // Handle lockout scenario
                    }
                }
                else
                {
                    // Handle failure
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //fetch the User Details
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    //If User does not exists, redirect to the Login Page
                    return RedirectToAction("Login", "Account");
                }

                // ChangePasswordAsync Method changes the user password
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or the current password is incorrect.
                // Add these errors to the ModelState and rerender ChangePassword view
                if (result.Succeeded)
                {
                    // Upon successfully changing the password refresh sign-in cookie
                    await signInManager.RefreshSignInAsync(user);

                    //Then redirect the user to the ChangePasswordConfirmation view
                    return RedirectToAction("index", "home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }
    }
}