using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AltenCancunResort.Data.Entities;

namespace AltenCancunResort.Data
{
    public interface IReservationRepository
    {
        // Creates a reservation and returns the ID.
        public Task<ReservationEntity> PlaceReservation(ReservationEntity reservation);

        // Updates the reservation dates.
        public Task<int> UpdateDatesReservation(int reservationID, DateTime startDate, DateTime endTime);

        // Checks availability for given dates
        public Task<bool> CheckAvailabilityForDates(DateTime startDate, DateTime endDate);

        // Cancels the selected reservation
        public Task<int> CancelReservation(int reservationID);

        // Gets the reservation by the selected ID
        public Task<ReservationEntity> GetReservationByID(int reservationID);

        // Retrieves the list of reservations for the selected guest
        public Task<List<ReservationEntity>> GetGuestReservations(int guestID);
    }
}