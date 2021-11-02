using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltenCancunResort.Data;
using AltenCancunResort.Data.Entities;
using AltenCancunResort.Models;

namespace AltenCancunResort.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IGuestRepository _guestRepository;

        public ReservationService(IReservationRepository reservationRepository, IGuestRepository guestRepository)
        {
            _reservationRepository = reservationRepository;
            _guestRepository = guestRepository;
        }



        // Creates a new reservation
        public async Task<ReservationOutput> CreateReservation(CreateReservationInput input)
        {
            DateTime startDate = input.StartDate.Date;
            DateTime endDate = input.EndDate.Date;

            // verify guest exists
            if(await _guestRepository.GetGuestByID(input.GuestID) == null)
            {
                return null;
            }

            // date range validation
            var validateObj = new ValidateReservationDatesInput
            {
                StartDate = startDate,
                EndDate = endDate
            };
            var validationResult = await ValidateReservationDates(validateObj);
            
            if(validationResult.IsValid)
            {
                // place reservation
                ReservationEntity reservation = new ReservationEntity
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    GuestID = input.GuestID,
                    Active = true // default active guest
                };
                var reservationEntity = await _reservationRepository.PlaceReservation(reservation);

                var reservationOutput = new ReservationOutput 
                {
                    ReservationID = reservationEntity.ReservationID,
                    StartDate = reservationEntity.StartDate,
                    EndDate = reservationEntity.EndDate,
                    Active = reservationEntity.Active,
                    GuestID = reservationEntity.GuestID
                };     
                return reservationOutput;
            }
            else
            {
                return null;
            }
        }

        // Gets the reservation for the given id
        public async Task<ReservationOutput> GetReservationById(ReservationIdInput input)
        {
            var reservation = await _reservationRepository.GetReservationByID(input.ReservationID);

            if(reservation != null)
            {
                var ReservationOutput = new ReservationOutput
                {
                    ReservationID = reservation.ReservationID,
                    StartDate = reservation.StartDate,
                    EndDate = reservation.EndDate,
                    GuestID = reservation.GuestID,
                    Active = reservation.Active
                };
                return ReservationOutput;
            }
            else
            {
                return null;
            }
        }

        // Checks availability in the selected dates
        public async Task<CheckAvailabilityOutput> CheckAvailability(CheckAvailabilityInput input)
        {
            bool available = await _reservationRepository.CheckAvailabilityForDates(input.StartDate.Date, input.EndDate.Date);
            var checkAvailability = new CheckAvailabilityOutput
            {
                IsAvailable = available
            };
            return checkAvailability;
        }

        // Marks reservation as canceled
        public async Task<UpdateReservationOutput> CancelReservation(ReservationIdInput input)
        {
            var reservation = await _reservationRepository.GetReservationByID(input.ReservationID);
            if(reservation != null)
            {
                int canceledCount = await _reservationRepository.CancelReservation(input.ReservationID);
                var cancelOutput = new UpdateReservationOutput
                {
                    IsReservationUpdated = canceledCount == 1
                };
                return cancelOutput;
            }
            else
            {
                return null;
            }
        }

        // Modifies the reservation dates
        public async Task<UpdateReservationOutput> ModifyReservationDates(ModifyReservationDatesInput input)
        {
            // validate reservation exists
            var reservation = await _reservationRepository.GetReservationByID(input.ReservationID);
            if(reservation == null)
            {
                return null;
            }

            // date range validation
            var validateObj = new ValidateReservationDatesInput
            {
                StartDate = input.StartDate.Date,
                EndDate = input.EndDate.Date
            };
            var validationResult = await ValidateReservationDates(validateObj);
            if(!validationResult.IsValid)
            {
                return null;
            }

            // Makes repository call to update
            var updatedCount = await _reservationRepository.UpdateDatesReservation(input.ReservationID, input.StartDate.Date, input.EndDate.Date);
            var updateOutput = new UpdateReservationOutput
            {
                IsReservationUpdated = updatedCount == 1
            };
            return updateOutput;
        }

        // Validate the reservation dates fullfil the required rules
        public async Task<ValidateReservationDatesOutput> ValidateReservationDates(ValidateReservationDatesInput input)
        {
            DateTime startDate = input.StartDate;
            DateTime endDate = input.EndDate;
            var validateDatesOutput = new ValidateReservationDatesOutput
            {
                IsValid = true
            };

            // verify correct date range
            if(endDate < startDate)
            {
                validateDatesOutput.IsValid = false;
                return validateDatesOutput;
            }

            // verify 3 days at maximum selected
            var intendedDays = (endDate - startDate).TotalDays;
            if(intendedDays > 3)
            {
                validateDatesOutput.IsValid = false;
                return validateDatesOutput;
            }

            // verify availability
            var isDateRangeAvailable = await _reservationRepository.CheckAvailabilityForDates(startDate, endDate);
            if(!isDateRangeAvailable)
            {
                validateDatesOutput.IsValid = false;
                return validateDatesOutput;
            }

            // verify selected dates are within 1 to 30 days range
            var Date30DaysFromToday = DateTime.Now.AddDays(30).Date;
            if(!(startDate > DateTime.Now.Date && 
                startDate <= Date30DaysFromToday && endDate <= Date30DaysFromToday))
            {
                validateDatesOutput.IsValid = false;
                return validateDatesOutput;
            }

            return validateDatesOutput;
        }
    
        // Get the active reservations for the given guest id
        public async Task<List<ReservationOutput>> GetActiveReservationsForGuest(GuestIdInput input)
        {
            // validate guest exist
            var guest = await _guestRepository.GetGuestByID(input.GuestID); 
            if(guest == null)
            {
                return null;
            }

            var reservations = await _reservationRepository.GetGuestReservations(input.GuestID);
            var reservationOutputList = reservations.Select(res =>
                    new ReservationOutput
                    {
                        GuestID = res.GuestID,
                        StartDate = res.StartDate,
                        EndDate = res.EndDate,
                        Active = res.Active,
                        ReservationID = res.ReservationID
                    }).ToList();
            return reservationOutputList;
        }
    }
}