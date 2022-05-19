using Astro_Nails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astro_Nails.Controllers
{
    public class ServiceController : Controller
    {
        private ApplicationContext _context;
        public ServiceController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Services.ToList());
        }

        public IActionResult Delete(int? id)
        {
            var Service = _context.Services.Find(id);
            if(Service != null)
            {
                _context.Services.Remove(Service);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Service service)
        {
            if(ModelState.IsValid)
            {
                _context.Services.Add(service);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var service = _context.Services.Find(id);
            if(service != null)
            {
                return View(service);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Service service)
        {
            if(ModelState.IsValid)
            {
                _context.Entry(service).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Edit");
        }
    }
}
