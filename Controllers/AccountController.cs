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
        private readonly UserManager<User> userManager;

        //signInManager will hold the SignInManager instance
        private readonly SignInManager<User> signInManager;

        private readonly RoleManager<IdentityRole> roleManager;
        
        //Both UserManager and SignInManager services are injected into the AccountController
        //using constructor injection
        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
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
                // Copy data from RegisterViewModel to User
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    SurName = model.SurName,
                    FirstName = model.FirstName,
                    FatherName = model.FatherName
                };

                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    // If the user is signed in and in the Admin role, then it is
                    // the Admin user that is creating a new user. 
                    // So redirect the Admin user to ListUsers action of Administration Controller
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Account");
                    }

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
                    // Обновляем SecurityStamp
                    await userManager.UpdateSecurityStampAsync(user);

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
                    // Обновляем дату последнего изменения пароля
                    user.LockoutEnd = DateTime.UtcNow;

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

        [AllowAnonymous]
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> IsUserNameAvailable(string userName)
        {
            //Check If the UserName Id is Already in the Database
            //var user = await userManager.FindByNameAsync(userName);
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"UserName {userName} is already in use.");
            }
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            List<IdentityRole> roles = await roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string UserId)
        {
            //First Fetch the User Details by UserId
            var user = await userManager.FindByIdAsync(UserId);

            //Check if User Exists in the Database
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);

            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);

            //Store all the information in the EditUserViewModel instance
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                FatherName = user.FatherName,
                Roles = userRoles
            };

            //Pass the Model to the View
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            //First Fetch the User by Id from the database
            var user = await userManager.FindByIdAsync(model.Id);

            //Check if the User Exists in the database
            if (user == null)
            {
                //If the User does not exists in the database, then return Not Found Error View
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                //If the User Exists, then proceed and update the data
                //Populate the user instance with the data from EditUserViewModel
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.FatherName = model.FatherName;

                //UpdateAsync Method will update the user data in the AspNetUsers Identity table
                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    //Once user data updated redirect to the ListUsers view
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    //In case any error, stay in the same view and show the model validation error
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            //First Fetch the User you want to Delete
            var user = await userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                // Handle the case where the user wasn't found
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }
            else
            {
                //Delete the User Using DeleteAsync Method of UserManager Service
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    // Handle a successful delete
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    // Handle failure
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View("ListUsers");
            }
        }
    }
}