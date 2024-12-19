using AllupMVC.Models;
using AllupMVC.Utilities.Enums;
using AllupMVC.Utilities.Extensions;
using AllupMVC.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string formattedName = userVM.FirstName.CheckName()?.ToString();

            if (formattedName == null)
            {
                ModelState.AddModelError(nameof(userVM.FirstName), "First Name has an incorrect format");

            }
            string formattedSurname = userVM.LastName.CheckName()?.ToString();
            if (formattedSurname == null)
            {
                ModelState.AddModelError(nameof(userVM.LastName), "Last Name has an incorrect format");
            }
            if (!userVM.EmailAddress.CheckEmail())
            {
                ModelState.AddModelError(nameof(userVM.EmailAddress), "Email is incorrect");
                return View();
            }

            AppUser user = new()
            {
                Name = formattedName,
                Email = userVM.EmailAddress,
                Surname = formattedSurname,
                UserName = userVM.UserName
            };

            IdentityResult result = await _userManager.CreateAsync(user, userVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                }
                return View();
            }

            await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());
            await _signInManager.SignInAsync(user, false);


            return RedirectToAction(nameof(HomeController.Index), "Home");


        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM userVM, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userVM.UserNameOrEmail || u.Email == userVM.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Username,Email or Password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, userVM.Password, userVM.IsPersistent, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account is locked out,please try again later");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username,Email or Password is incorrect");
            }

            if (returnUrl == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(returnUrl);

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }
        //    return RedirectToAction(nameof(HomeController.Index), "Home");
        //}

    }
}
