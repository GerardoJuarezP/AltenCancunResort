using System;
using System.Threading.Tasks;
using AltenCancunResort.Data;
using AltenCancunResort.Models;
using Microsoft.AspNetCore.Mvc;

namespace AltenCancunResort.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IGuestRepository _guestRepository;

        public ReservationController(IReservationRepository reservationRepository, IGuestRepository guestRepository)
        {
            _reservationRepository = reservationRepository;
            _guestRepository = guestRepository;
        }



        // Reservation create action
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Reservation reservation)
        {
            DateTime startDate = reservation.StartDate.Date;
            DateTime endDate = reservation.EndDate.Date;

            // verify guest exists
            if(await _guestRepository.GetGuestByID(reservation.GuestID) == null)
            {
                return NotFound("Guest not found.");
            }

            // date range validation
            var validationResult = await ReservationDatesRangeValidation(startDate, endDate);
            if(!typeof(OkResult).IsAssignableFrom(validationResult.GetType()))
            {
                return validationResult;
            }

            // place reservation
            await _reservationRepository.PlaceReservation(reservation);
            
            // returns the created reservation object
            return CreatedAtRoute("GetReservationByID", new { reservationID = reservation.ReservationID }, reservation);
        }

        // Checks availability
        [HttpGet("checkAvailability/{start:DateTime}/{end:DateTime}")]
        public async Task<IActionResult> CheckAvailability(DateTime start, DateTime end)
        {
            bool available = await _reservationRepository.CheckAvailabilityForDates(start.Date, end.Date);
            return Ok(available);
        }

        // Get list of guest's reservations
        [HttpGet("{guestID:int}")]
        public async Task<IActionResult> GetAllReservationsForGuest(int guestID)
        {
            var reservations = await _reservationRepository.GetGuestReservations(guestID);
            return Ok(reservations);
        }

        // Get reservation by id
        [HttpGet("reservation/{reservationID:int}", Name = "GetReservationByID")]
        public async Task<IActionResult> GetReservationByID(int reservationID)
        {
            var reservations = await _reservationRepository.GetReservationByID(reservationID);
            return Ok(reservations);
        }
    
        // Reservation create action
        [HttpPatch("cancel/{reservationID:int}")]
        public async Task<IActionResult> CancelReservation(int reservationID)
        {
            await _reservationRepository.CancelReservation(reservationID);
            return NoContent();
        }
    
        // Reservation create action
        [HttpPatch("updateDates/{reservationID:int}/{startDate:DateTime}/{endDate:DateTime}")]
        public async Task<IActionResult> ModifyReservationDates(int reservationID, DateTime startDate, DateTime endDate)
        {
            // date range validation
            var validationResult = await ReservationDatesRangeValidation(startDate, endDate);
            if(!typeof(OkResult).IsAssignableFrom(validationResult.GetType()))
            {
                return validationResult;
            }
            
            await _reservationRepository.UpdateDatesReservation(reservationID, startDate, endDate);
            return NoContent();
        }

        // Validate the dates in the given range
        private async Task<IActionResult> ReservationDatesRangeValidation(DateTime startDate, DateTime endDate)
        {
            // verify correct date range
            if(endDate < startDate)
            {
                return BadRequest("Incorrect date range selected.");
            }

            // verify 3 days at maximum selected
            var intendedDays = (endDate - startDate).TotalDays;
            if(intendedDays > 3)
            {
                return BadRequest("Maximum 3 days per reservation.");
            }

            // verify availability
            var isDateRangeAvailable = await _reservationRepository.CheckAvailabilityForDates(startDate, endDate);
            if(!isDateRangeAvailable)
            {
                return BadRequest("Selected dates-range not available.");
            }

            // verify selected dates are within 1 to 30 days range
            var Date30DaysFromToday = DateTime.Now.AddDays(30).Date;
            if(!(startDate > DateTime.Now.Date && 
                startDate <= Date30DaysFromToday && endDate <= Date30DaysFromToday))
            {
                return BadRequest("Dates-range has to start tomorrow at least and between 30 days from today.");
            }

            return Ok();
        }
    }
}