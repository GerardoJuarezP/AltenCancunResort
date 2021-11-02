using System.Collections.Generic;
using System.Threading.Tasks;
using AltenCancunResort.Models;

namespace AltenCancunResort.Services
{
    public interface IReservationService
    {
        // Creates a new reservation
        public Task<ReservationOutput> CreateReservation(CreateReservationInput input);

        // Checks availability in the selected dates
        public Task<CheckAvailabilityOutput> CheckAvailability(CheckAvailabilityInput input);

        // Gets the reservation for the given id
        public Task<ReservationOutput> GetReservationById(ReservationIdInput input);

        // Marks reservation as canceled
        public Task<UpdateReservationOutput> CancelReservation(ReservationIdInput input);

        // Modifies the reservation dates
        public Task<UpdateReservationOutput> ModifyReservationDates(ModifyReservationDatesInput input);

        // Validate the reservation dates fullfil the required rules
        public Task<ValidateReservationDatesOutput> ValidateReservationDates(ValidateReservationDatesInput input);

        // Get the active reservations for the given guest id
        public Task<List<ReservationOutput>> GetActiveReservationsForGuest(GuestIdInput input);
    }
}