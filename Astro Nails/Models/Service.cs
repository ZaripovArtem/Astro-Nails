using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astro_Nails.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Order> Orders { get; set; }
        public Service()
        {
            Orders = new List<Order>();
        }
    }
}
