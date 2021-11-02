using System.Threading.Tasks;
using AltenCancunResort.Data.Entities;

namespace AltenCancunResort.Data
{
    public interface IGuestRepository
    {
        // Creates a guest and returns the ID
        public Task<GuestEntity> CreateGuest(GuestEntity guest);

        // Retrieves a guest by ID
        public Task<GuestEntity> GetGuestByID(int guestID);

        // Enables/Disables selected guest
        public Task<int> StatusActiveGuest(int guestID, bool status);
    }
}