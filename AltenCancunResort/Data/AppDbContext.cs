using Microsoft.EntityFrameworkCore;
using AltenCancunResort.Data.Entities;

namespace AltenCancunResort.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<GuestEntity> Guests { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) {}
    }
}
