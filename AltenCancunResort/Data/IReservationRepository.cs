using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AltenCancunResort.Models;

namespace AltenCancunResort.Data
{
    public interface IReservationRepository
    {
        // Creates a reservation and returns the ID.
        public Task<int> PlaceReservation(Reservation reservation);

        // Updates the reservation dates.
        public Task UpdateDatesReservation(int reservationID, DateTime startDate, DateTime endTime);

        // Retrieves the list of reservations for the selected guest.
        public Task<List<Reservation>> GetGuestReservations(int guestID);

        // Checks availability for given dates
        public Task<bool> CheckAvailabilityForDates(DateTime startDate, DateTime endDate);

        // Cancels the selected reservation
        public Task CancelReservation(int reservationID);

        // Gets the reservation by the selected ID
        public Task<Reservation> GetReservationByID(int reservationID);
    }
}