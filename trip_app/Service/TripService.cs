using trip_app.DTO;
using trip_app.OutputFolder;
using trip_app.Repository;
using TripApp.Application.Mappers;

namespace trip_app.Service;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;

    public TripService(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<PaginatedResult<TripDTO>> GetPaginatedTripsAsync(int page = 1, int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 10) page = 10;
        var result = await _tripRepository.GetPaginatedTripsAsync(page, pageSize);

        var mappedTrips = new PaginatedResult<TripDTO>
        {
            AllPages = result.AllPages,
            Data = result.Data.Select(trip => trip.MapToGetTripDto()).ToList(),
            PageNum = result.PageNum,
            PageSize = result.PageSize
        };

        return mappedTrips;
    }

    public async Task<List<TripDTO>> GetAllTripsAsync()
    {
        var trips = await _tripRepository.GetAllTripsAsync();

        // Sort by StartDate in descending order before mapping
        var sortedTrips = trips.OrderByDescending(trip => trip.DateFrom).ToList();

        var mappedTrips = sortedTrips.Select(trip => trip.MapToGetTripDto()).ToList();
        return mappedTrips;
    }

}