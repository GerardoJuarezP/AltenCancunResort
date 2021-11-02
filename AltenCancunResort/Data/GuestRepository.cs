using System.Linq;
using System.Threading.Tasks;
using AltenCancunResort.Models;
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
        public async Task<int> CreateGuest(Guest guest)
        {
            _dbContext.Guests.Add(guest);
            await _dbContext.SaveChangesAsync();
            return guest.GuestID;
        }

        // Retrieves a guest by ID
        public Task<Guest> GetGuestByID(int guestID)
        {
            return _dbContext.Guests.Where(guest => guest.GuestID == guestID).FirstOrDefaultAsync<Guest>();
        }

        // Enables/Disables selected guest
        public Task<int> StatusActiveGuest(int guestID, bool activate)
        {
            Guest selectedGuest = _dbContext.Guests.Where(guest => guest.GuestID == guestID).FirstOrDefault<Guest>();
            if(selectedGuest != null)
            {
                selectedGuest.Active = activate;
                return _dbContext.SaveChangesAsync();
            }
            else
            {
                return null;
            }
        }
    }
}