using Astro_Nails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astro_Nails.Controllers
{
    public class OrderController : Controller
    {
        ApplicationContext _context;

        public OrderController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IQueryable<User> users = _context.Users
                .Where(u => u.RoleId == 1 || u.RoleId > 2);
            IQueryable<Service> services = _context.Services
                .OrderBy(i => i.Id);
            ViewBag.Services = new SelectList(services, "Id", "Name");
            ViewBag.Users = new SelectList(users, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Index(Order order)
        {
            IQueryable<User> users = _context.Users
                .Where(u => u.RoleId == 3);
           
            ViewBag.Users = new SelectList(users, "Id", "Name");

            if(order.DateTime_Order.Hour > 21 || order.DateTime_Order.Hour < 8)
            {
                ModelState.AddModelError("DateTime_Order", "Нерабочее время");
            }

            DateTime order_start = new DateTime();
            try
            {
                order_start = order.DateTime_Order.AddHours(-2);
            }
             catch
            {
                ModelState.AddModelError("DateTime_Order", "");
            }
              var order_end = order.DateTime_Order.AddHours(2);




            // Проверка на то, есть ли запись за 30 минут
            IQueryable<Order> orders_time30 = _context.Orders
                .Where(o => o.UserId == order.UserId && (o.DateTime_Order >= order_start && o.DateTime_Order <= order_end));
            if (orders_time30.Count() != 0)
            {
                ModelState.AddModelError("DateTime_Order", "Запись на данное время уже существует");
            }

            // Если пользователь авторизирован - поиск его ФИО
            IQueryable<User> user_fio = _context.Users
               .Where(u => u.Phone == User.Identity.Name);

            if(order.DateTime_Order.DayOfWeek == DayOfWeek.Saturday || order.DateTime_Order.DayOfWeek == DayOfWeek.Sunday)
            {
                ModelState.AddModelError("DateTime_Order", "Выходной день");
            }

            if(order.DateTime_Order < DateTime.Now.AddHours(-2))
            {
                ModelState.AddModelError("DateTime_Order", "Запись проводится минимум за 2 часа до сеанса");
            }

            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    foreach(var p in user_fio)
                    {
                        order.Name = p.Name;
                        order.Surname = p.Surname;
                        order.Patronomic = p.Patronomic;
                    }
                    order.ClientPhone = User.Identity.Name;
                    order.DateTime_Now = DateTime.Now;
                    order.Service_State = "Оформлено";
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                }
                else
                {
                    order.DateTime_Now = DateTime.Now;
                    order.Service_State = "Оформлено";
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                }
                
                return RedirectToAction("Index", "Home");
            }
            return View(order);
        }
    }
}
