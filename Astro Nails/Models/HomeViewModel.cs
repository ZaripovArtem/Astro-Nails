﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astro_Nails.Models
{
    public class HomeViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public SelectList Maser { get; set; }
    }
}
