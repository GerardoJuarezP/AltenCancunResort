using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltenCancunResort.Models;
using Microsoft.EntityFrameworkCore;

namespace AltenCancunResort.Data
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _dbContext;

        public ReservationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        // Cancels the selected reservation
        public Task CancelReservation(int reservationID)
        {
            Reservation selectedReservation = _dbContext.Reservations.Where(res => res.ReservationID == reservationID).FirstOrDefault<Reservation>();
            if(selectedReservation != null)
            {
                selectedReservation.Active = false;
                return _dbContext.SaveChangesAsync();
            }
            else
            {
                return null;
            }
        }

        // Validate if there is not reservations for the selected dates.
        public async Task<bool> CheckAvailabilityForDates(DateTime startDate, DateTime endDate)
        {
            var conflictsList = await (from res in _dbContext.Reservations
                            where ((startDate >= res.StartDate && startDate <= res.EndDate) ||
                                    (endDate >= res.StartDate && endDate <= res.EndDate)) &&
                                    res.Active == true
                            select res).ToListAsync();
            int conflictsCount = conflictsList.Count;

            return conflictsCount == 0;
        }

        // Retrieves the list of reservations for the selected guest
        public Task<List<Reservation>> GetGuestReservations(int guestID)
        {
            var guestReservations = (from res in _dbContext.Reservations
                            where res.GuestID == guestID && res.Active == true
                            select res).ToListAsync<Reservation>();

            return guestReservations;
        }

        // Creates a reservation and returns the ID.
        public async Task<int> PlaceReservation(Reservation reservation)
        {
            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync();
            return reservation.ReservationID;
        }

        // Updates the reservation dates.
        public Task UpdateDatesReservation(int reservationID, DateTime startDate, DateTime endTime)
        {
            Reservation selectedReservation = _dbContext.Reservations.Where(res => res.ReservationID == reservationID).FirstOrDefault<Reservation>();
            if(selectedReservation != null)
            {
                selectedReservation.StartDate = startDate;
                selectedReservation.EndDate = endTime;
                return _dbContext.SaveChangesAsync();
            }
            else
            {
                return null;
            }
        }

        // Gets the reservation by the selected ID
        public Task<Reservation> GetReservationByID(int reservationID)
        {
            return _dbContext.Reservations.Where(res => res.ReservationID == reservationID).FirstOrDefaultAsync<Reservation>();
        }
    }
}