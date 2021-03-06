using Astro_Nails.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astro_Nails.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationContext _context;
        public AdminController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Пользователи, RoleId > 2 
            IQueryable<User> users = _context.Users
                .Where(u => u.RoleId > 2);
            return View(users);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var user = _context.Users.Find(id);
            if(user != null)
            {
                
                    SelectList role = new SelectList(_context.Roles, "Id", "Name");
                    ViewBag.Roles = role;
                    var users = _context.Users.Find(id);
                    return View(users);
                
                
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            IQueryable<User> check_phone = _context.Users
                    .Where(c => c.Phone == user.Phone);
            if(check_phone.Count() > 0)
            {
                ModelState.AddModelError("Phone", "Данный номер уже используется");
            }

            if (ModelState.IsValid)
            {
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Edit");
        }

        public IActionResult Delete(int? id)
        {

            var user = _context.Users.Find(id);

            if(user != null)
            {
                IQueryable<Order> orders = _context.Orders.Where(u => u.UserId == user.Id);
                if (orders != null)
                {
                    foreach (var o in orders)
                    {
                        Order order = _context.Orders.Find(o.Id);
                        order.UserId = 777;
                        _context.Entry(order).State = EntityState.Modified;
                    }
                }

                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteAll(int? id)
        {
            Order order = _context.Orders.Find(id);
            if(order != null)
            {
                order.Service_State = "Отклонено";
                _context.Entry(order).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult Create()
        {
            SelectList role = new SelectList(_context.Roles, "Id", "Name");
            ViewBag.Roles = role;
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            IQueryable<User> check_phone = _context.Users
                    .Where(c => c.Phone == user.Phone);
            if (check_phone.Count() > 0)
            {
                ModelState.AddModelError("Phone", "Данный номер уже используется");
            }

            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }
        public IActionResult All()
        {
            var order = _context.Orders.Include(p => p.User).Include(d => d.Service);
            return View(order.ToList());
        }
    }
}
