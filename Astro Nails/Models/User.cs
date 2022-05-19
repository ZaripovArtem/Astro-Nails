using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Astro_Nails.Models
{
    public class User
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronomic { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Phone { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Password { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public List<Order> Orders { get; set; }
        public User()
        {
            Orders = new List<Order>();
        }
    }
}
