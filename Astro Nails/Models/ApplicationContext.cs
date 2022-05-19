using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astro_Nails.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Service> Services { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Service s1 = new Service { Id = 1, Name = "Услуга 1" };
            Service s2 = new Service { Id = 2, Name = "Услуга 2" };

            

            Role AdminRole = new Role { Id = 1, Name = "Admin"};
            Role UserRole = new Role { Id = 2, Name = "User" };
            Role MasterRole = new Role { Id = 3, Name = "Master" };
            Role TopMasterRole = new Role { Id = 4, Name = "Top Master" };

            User Admin = new User { Id = 1, Phone = "admin@mail.ru", Name = "Аделя", Surname = "Хамидуллина", Patronomic = "qe", Password = "12345", RoleId = 1 };
            User User = new User { Id = 2, Phone = "user@mail.ru", Name = "Аделя", Surname = "Хамидуллина", Patronomic="qe", Password = "qweasd", RoleId = 2 };
            User Master = new User { Id = 3, Phone = "master@mail.ru", Name = "Имя", Surname = "Фамилия", Patronomic = "Отчество", Password = "qweasd", RoleId = 3 };

            modelBuilder.Entity<Role>().HasData(new Role[] { AdminRole, UserRole, MasterRole });
            modelBuilder.Entity<User>().HasData(new User[] { Admin, User, Master });
            modelBuilder.Entity<Service>().HasData(new Service[] { s1, s2 });
            base.OnModelCreating(modelBuilder);
        }
    }
}
