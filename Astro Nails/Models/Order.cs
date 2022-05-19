using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Astro_Nails.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateTime_Now { get; set; }
        public DateTime DateTime_Order { get; set; }
        public int? UserId { get; set; } // id мастера
        public int? ServiceId { get; set; }
        public string Service_State { get; set; } // Состояние услуги (оформлен, отменен)
        public string ClientPhone { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronomic { get; set; }
        public Service Service { get; set; }
        public User User { get; set; }
    }
}
