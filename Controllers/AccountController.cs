using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Controllers
{
    [Authorize]     // Помічаємо що доступ до контролеру тільки для авторизованих користувачів
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;



        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)    // Даємо форму авторизації
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)    // Пробуємо авторизувати юзера
        {
            // Перевірка валідності данних в формі
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();    // Вийти з поточного користувача (для успішного входу в admin)
                    
                    // Спроба увійти в користувача, під вказаним паролем
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                        return Redirect(returnUrl ?? "/"); // Повертаємо юзера на сторінку в яку він хотів увійти
                }

                // Не знайдено користувача, або пароль не правильний
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Неправильний логін або пароль");

            }

            return View(model); // Повертаємо модель (з помилкою якщо є проблеми)
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();        // Вийти з користувача
            return RedirectToAction("Index", "Home");   // Перенаправити на головну сторінку
        }
    }
}
