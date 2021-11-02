using Microsoft.EntityFrameworkCore;
using AltenCancunResort.Models;

namespace AltenCancunResort.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) {}
    }
}
