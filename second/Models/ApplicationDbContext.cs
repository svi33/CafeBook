using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace second.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }

        public DbSet<Hall> Halls { get; set; }
        public DbSet<ApplicationUser> UsersList { get; set; }
        public DbSet<Restaurants> RestaurantsList { get; set; }
        public DbSet<Pictures> TablesImages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Tables> TablesList { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


        }
    }
}
