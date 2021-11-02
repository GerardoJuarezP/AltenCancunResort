using System.Threading.Tasks;
using AltenCancunResort.Models;

namespace AltenCancunResort.Services
{
    public interface IGuestService
    {
        // Service to create a new guest
        public Task<GuestOutput> CreateGuest(CreateGuestInput input);

        // Update active guest status
        public Task<UpdateGuestStatusOutput> UpdateGuestStatus(UpdateGuestInput input);

        // Service to retrieve the Guest given the id
        public Task<GuestOutput> GetGuestById(GuestIdInput input);
    }
}