using Astro_Nails.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astro_Nails.Controllers
{
    public class MasterController : Controller
    {
        private ApplicationContext _context;
        public MasterController(ApplicationContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                IQueryable<User> this_user = _context.Users
                .Where(u => u.Phone == User.Identity.Name);
                foreach (var p in this_user)
                {
                    IQueryable<Order> master_order = _context.Orders
                    .Where(o => o.UserId == p.Id);
                    return View(master_order);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
