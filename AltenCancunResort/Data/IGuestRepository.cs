using System.Threading.Tasks;
using AltenCancunResort.Models;

namespace AltenCancunResort.Data
{
    public interface IGuestRepository
    {
        // Creates a guest and returns the ID
        public Task<int> CreateGuest(Guest guest);

        // Retrieves a guest by ID
        public Task<Guest> GetGuestByID(int guestID);

        // Enables/Disables selected guest
        public Task<int> StatusActiveGuest(int guestID, bool activate);
    }
}