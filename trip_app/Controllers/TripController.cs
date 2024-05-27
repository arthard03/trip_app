using Microsoft.AspNetCore.Mvc;
using trip_app.DTO;
using trip_app.Exceptions;

using trip_app.Service;

namespace trip_app.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripRepository;
        public TripController(ITripService tripRepository)
        {
            _tripRepository = tripRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _tripRepository.GetPaginatedTripsAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("clients")] 
        public async Task<IActionResult> GetTripsClients()
        {
            var result = await _tripRepository.GetAllTripsAsync();
            return Ok(result);
        }
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientPostDTO clientDto)
        {
            try
            {
                var result = await _tripRepository.AssignClientToTripAsync(clientDto, idTrip);
                if (!result)
                {
                    return BadRequest("Unable to assign client to trip.");
                }

                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            try
            {
                var result = await _tripRepository.DeleteClientAsync(idClient);
        
                if (!result)
                {
                    return NotFound("Client not found.");
                }
        
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}