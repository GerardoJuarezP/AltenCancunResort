using System.Threading.Tasks;
using AltenCancunResort.Data;
using AltenCancunResort.Models;
using Microsoft.AspNetCore.Mvc;

namespace AltenCancunResort.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly IGuestRepository _repository;

        public GuestController(IGuestRepository repository)
        {
            _repository = repository;
        }

        // Guest create action
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Guest guest)
        {
            await _repository.CreateGuest(guest);
            return CreatedAtRoute("GetByID", new { guestID = guest.GuestID }, guest);
        }

        // Retrieves the guest for the selected ID
        [HttpGet("{guestID:int}", Name = "GetByID")]
        public async Task<IActionResult> GetByID(int guestID)
        {
            Guest guest= await _repository.GetGuestByID(guestID);
            if(guest == null)
            {
                return NotFound();
            }
            return Ok(guest);
        }

        [HttpPatch("{id:int}/{status:bool}")]
        public async Task<IActionResult> UpdateGuestStatus(int id, bool status)
        {
            await _repository.StatusActiveGuest(id, status);
            return NoContent();
        }
    }
}