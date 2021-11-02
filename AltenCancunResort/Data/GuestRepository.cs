using System;
using System.Linq;
using System.Threading.Tasks;
using AltenCancunResort.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AltenCancunResort.Data
{
    public class GuestRepository : IGuestRepository
    {
        private readonly AppDbContext _dbContext;

        public GuestRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        // Creates a guest and returns the ID
        public async Task<GuestEntity> CreateGuest(GuestEntity guest)
        {
            //default status active
            _dbContext.Guests.Add(guest);
            await _dbContext.SaveChangesAsync();
            return guest;
        }

        // Retrieves a guest by ID
        public Task<GuestEntity> GetGuestByID(int guestID)
        {
            return _dbContext.Guests.Where(guest => guest.GuestID == guestID).FirstOrDefaultAsync<GuestEntity>();
        }

        // Enables/Disables selected guest
        public async Task<int> StatusActiveGuest(int guestID, bool status)
        {
            var selectedGuest = await _dbContext.Guests.Where(guest => guest.GuestID == guestID).FirstOrDefaultAsync<GuestEntity>();
            selectedGuest.Active = status;
            return await _dbContext.SaveChangesAsync();
        }
    }
}