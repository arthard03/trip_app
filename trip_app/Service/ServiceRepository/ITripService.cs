using trip_app.DTO;
using trip_app.OutputFolder;

namespace trip_app.Service;

public interface ITripService
{
    Task<PaginatedResult<TripDTO>> GetPaginatedTripsAsync(int page = 1, int pageSize = 10);
    Task<List<TripDTO>> GetAllTripsAsync();
}