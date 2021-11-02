using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltenCancunResort.Data.Entities;
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
        public async Task<int> CancelReservation(int reservationID)
        {
            var selectedReservation = await _dbContext.Reservations.Where(res => res.ReservationID == reservationID).FirstOrDefaultAsync<ReservationEntity>();
            selectedReservation.Active = false;
            return await _dbContext.SaveChangesAsync();
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
        public Task<List<ReservationEntity>> GetGuestReservations(int guestID)
        {
            var guestReservations = (from res in _dbContext.Reservations
                            where res.GuestID == guestID && res.Active == true
                            select res).ToListAsync<ReservationEntity>();

            return guestReservations;
        }

        // Creates a reservation and returns the ID.
        public async Task<ReservationEntity> PlaceReservation(ReservationEntity reservation)
        {
            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync();
            return reservation;
        }

        // Updates the reservation dates.
        public async Task<int> UpdateDatesReservation(int reservationID, DateTime startDate, DateTime endTime)
        {
            var selectedReservation = await _dbContext.Reservations.Where(res => res.ReservationID == reservationID).FirstOrDefaultAsync<ReservationEntity>();
            if(selectedReservation != null)
            {
                selectedReservation.StartDate = startDate;
                selectedReservation.EndDate = endTime;
                return await _dbContext.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        // Gets the reservation by the selected ID
        public Task<ReservationEntity> GetReservationByID(int reservationID)
        {
            return _dbContext.Reservations.Where(res => res.ReservationID == reservationID).FirstOrDefaultAsync<ReservationEntity>();
        }
    }
}