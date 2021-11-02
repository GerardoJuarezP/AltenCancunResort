using System.Threading.Tasks;
using AltenCancunResort.Data;
using AltenCancunResort.Data.Entities;
using AltenCancunResort.Models;
using System.Linq;

namespace AltenCancunResort.Services
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _repository;

        public GuestService(IGuestRepository repository)
        {
            _repository = repository;
        }



        // Service to create a new guest
        public async Task<GuestOutput> CreateGuest(CreateGuestInput input)
        {
            // dto mapping
            var guest = new GuestEntity{
                FirstName = input.FirstName,
                LastName = input.LastName,
                Active = true // default active
            };

            // repository call to create guest
            guest = await _repository.CreateGuest(guest);

            // form output
            var guestOutput = new GuestOutput{
                GuestID = guest.GuestID,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Active = guest.Active
            };
            
            return guestOutput;
        }

        // Service to retrieve the Guest given the id
        public async Task<GuestOutput> GetGuestById(GuestIdInput input)
        {
            // repository call
            var guest = await _repository.GetGuestByID(input.GuestID);

            if(guest != null)
            {
                // output dto mapping
                GuestOutput guestOutput = new GuestOutput{
                    GuestID = guest.GuestID,
                    FirstName = guest.FirstName,
                    LastName = guest.LastName,
                    Active = guest.Active
                };
                return guestOutput;
            }
            else
            {
                return null;
            }        
        }

        // Update active guest status
        public async Task<UpdateGuestStatusOutput> UpdateGuestStatus(UpdateGuestInput input)
        {
            var guest = await _repository.GetGuestByID(input.GuestID);
            if(guest != null)
            {
                int updatedCount = await _repository.StatusActiveGuest(input.GuestID, input.Status);
                var updateGuestOutput = new UpdateGuestStatusOutput
                {
                    IsGuestUpdated = updatedCount == 1
                };
                return updateGuestOutput;
            }
            else
            {
                return null;
            }
        }
    }
}