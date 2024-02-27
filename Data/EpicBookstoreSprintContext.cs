using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EpicBookstoreSprint.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EpicBookstoreSprint.Data
{
    public class EpicBookstoreContext : IdentityDbContext<DefaultUser>
    {
        public EpicBookstoreContext (DbContextOptions<EpicBookstoreContext> options)
            : base(options)
        {
        }

        public DbSet<Books> Book { get; set; } = default!;
        public DbSet<CartItems> CartItem { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
