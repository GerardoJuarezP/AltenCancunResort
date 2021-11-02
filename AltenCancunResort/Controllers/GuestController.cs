using System;
using System.Threading.Tasks;
using AltenCancunResort.Models;
using AltenCancunResort.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AltenCancunResort.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly IGuestService _service;
        private readonly ILogger<GuestController> _logger;

        public GuestController(IGuestService service, ILogger<GuestController> logger)
        {
            _service = service;
            _logger = logger;
        }



        // Guest create action
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGuestInput input)
        {
            try
            {
                var outputResult = await _service.CreateGuest(input);
                var idInputObj = new GuestIdInput
                {
                    GuestID = outputResult.GuestID
                };
                return CreatedAtRoute("GetByID", idInputObj, outputResult);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in Create GuestController: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Retrieves the guest for the selected ID
        [HttpGet("{GuestID:int}", Name = "GetByID")]
        public async Task<IActionResult> GetByID([FromRoute] GuestIdInput input)
        {
            try
            {
                var guest = await _service.GetGuestById(input);
                if(guest == null)
                {
                    return NotFound();
                }
                return Ok(guest);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in GetByID GuestController: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Updates the guest status
        [HttpPatch("{GuestID:int}/{Status:bool}")]
        public async Task<IActionResult> UpdateGuestStatus([FromRoute] UpdateGuestInput input)
        {
            try
            {
                var outputResult = await _service.UpdateGuestStatus(input);
                if(outputResult != null)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("Entered guest not found.");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in UpdateGuestStatus GuestController: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}