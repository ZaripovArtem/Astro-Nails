using Astro_Nails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Astro_Nails.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _context;

        public AccountController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Phone == model.Phone && u.Password == model.Password);
                if(user != null)
                {
                    await Authenticate(user); // аутентификация
                    if(user.RoleId == 2)
                        return RedirectToAction("Index", "Account");
                    if (user.RoleId == 1)
                        return RedirectToAction("Index", "Admin");
                    if (user.RoleId > 2)
                        return RedirectToAction("Index", "Master");

                }
                else
                {
                    ModelState.AddModelError("Password", "Неверный логин или пароль");
                }
            }
            
            return View(model);
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
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == model.Phone);
                if(user == null)
                {
                    user = new User
                    {
                        Phone = model.Phone,
                        Password = model.Password,
                        Name = model.Name,
                        Surname = model.Surname,
                        Patronomic = model.Patronomic
                    };

                    Role UserRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

                    if (UserRole != null)
                        user.Role = UserRole;

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
            }
            
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Phone),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                IQueryable<User> localUser = _context.Users
                    .Where(u => u.Phone == User.Identity.Name);
                return View(localUser);


            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await _context.Users.FindAsync(id);

                if (user.Phone == User.Identity.Name)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            IQueryable<User> user_login = _context.Users
                .Where(u => u.Phone == User.Identity.Name);
            foreach(var p in user_login)
            {
                user.RoleId = p.RoleId;
                
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Entry()
        {
            if(User.Identity.IsAuthenticated)
            {
                var orders = _context.Orders.Include(u => u.User)
                    .Where(u => u.ClientPhone == User.Identity.Name);
                if(orders.Count() == 0)
                {
                    ViewBag.Null = "Заказов нет";
                }
                return View(orders.ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Entry(int? id)
        {
            Order order = _context.Orders.Find(id);
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Delete(int? id)
        {
            var qwe = _context.Orders.Find(id);
            if(qwe != null)
            {
                _context.Orders.Remove(qwe);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
