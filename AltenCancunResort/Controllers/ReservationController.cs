using System;
using System.Linq;
using System.Threading.Tasks;
using AltenCancunResort.Data;
using AltenCancunResort.Models;
using AltenCancunResort.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AltenCancunResort.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IGuestService _guestService;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(IReservationService reservationService, 
                                    IGuestService guestService,
                                    ILogger<ReservationController> logger)
        {
            _reservationService = reservationService;
            _guestService = guestService;
            _logger = logger;
        }



        // Reservation create action
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationInput input)
        {
            try
            {
                var reservationCreatedOutput = await _reservationService.CreateReservation(input);
                if(reservationCreatedOutput == null)
                {
                    return BadRequest();
                }
                var reservationIdInput = new ReservationIdInput
                {
                    ReservationID = reservationCreatedOutput.ReservationID
                };
                return CreatedAtRoute("GetReservationByID", reservationIdInput, reservationCreatedOutput);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in Create ReservationController: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            
        }

        // Get reservation by id
        [HttpGet("reservation/{ReservationID:int}", Name = "GetReservationByID")]
        public async Task<IActionResult> GetReservationByID([FromRoute] ReservationIdInput input)
        {
            try
            {
                var reservation = await _reservationService.GetReservationById(input);
                if(reservation == null)
                {
                    return NotFound();
                }
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetReservationByID ReservationController: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Checks availability
        [HttpGet("checkAvailability/{StartDate:DateTime}/{EndDate:DateTime}")]
        public async Task<IActionResult> CheckAvailability([FromRoute] CheckAvailabilityInput input)
        {
            var available = await _reservationService.CheckAvailability(input);
            if(available.IsAvailable)
            {
                return Ok();
            }
            else
            {
                return NotFound("The given dates are not available.");
            }
        }
    
        // Cancel the selected reservation
        [HttpPatch("cancel/{ReservationID:int}")]
        public async Task<IActionResult> CancelReservation([FromRoute] ReservationIdInput input)
        {
            try
            {
                var outputResult = await _reservationService.CancelReservation(input);
                if(outputResult != null)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("Entered reservation not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CancelReservation ReservationController: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    
        // Modify the reservation dates
        [HttpPatch("updateDates/{ReservationID:int}/{StartDate:DateTime}/{EndDate:DateTime}")]
        public async Task<IActionResult> ModifyReservationDates([FromRoute] ModifyReservationDatesInput input)
        {
            try
            {
                // date range validation
                var validationResult = await _reservationService.ModifyReservationDates(input);
                if(validationResult != null)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ModifyReservationDates ReservationController: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Get list of guest's reservations
        [HttpGet("{GuestID:int}")]
        public async Task<IActionResult> GetAllReservationsForGuest([FromRoute]GuestIdInput input)
        {
            var reservations = await _reservationService.GetActiveReservationsForGuest(input);
            if(reservations != null)
            {
                return Ok(reservations);
            }
            else
            {
                return NotFound();
            }
        }
    }
}