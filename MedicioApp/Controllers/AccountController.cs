using Medicio.Core.Models;
using MedicioApp.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace MedicioApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
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
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return View(registerDto);
            }
            User user = new User()
            {
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };
            var result= await _userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);

                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "User");
            return RedirectToAction("Login");

        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(loginDto.EmailOrUsername);
            if(user == null)
            {
                user = await _userManager.FindByNameAsync(loginDto.EmailOrUsername);
                if(user == null)
                {
                    ModelState.AddModelError("", "Email/Usernema ve ya password yanlisdir");
                    return View();
                }
            }
            await _signInManager.SignInAsync(user, loginDto.IsRemember);
            var roles = await _userManager.GetRolesAsync(user);

            if(roles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role = new IdentityRole("Admin");
            IdentityRole role1 = new IdentityRole("User");
            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role);
            return Ok();

        }
    }
}
