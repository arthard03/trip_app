using Microsoft.AspNetCore.Mvc;
using trip_app.OutputFolder;
using trip_app.Repository;
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

        [HttpGet("trips")]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _tripRepository.GetPaginatedTripsAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("trips/clients")] 
        public async Task<IActionResult> GetTripsClients()
        {
            var result = await _tripRepository.GetAllTripsAsync();
            return Ok(result);
        }
        // // POST: api/Clients
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Client>> PostClient(Client client)
        // {
        //     return null;
        // }
        //
        // // DELETE: api/Clients/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteClient(int id)
        // {
        //     
        //     return NoContent();
        // }



    }
}