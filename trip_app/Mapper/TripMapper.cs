

using trip_app.DTO;
using trip_app.OutputFolder;

namespace TripApp.Application.Mappers;

public static class TripMapper
{
    public static TripDTO MapToGetTripDto(this Trip trip)
    {
        return new TripDTO
        {
            Name = trip.Name,
            DateFrom = trip.DateFrom,
            DateTo = trip.DateTo,
            Description = trip.Description,
            MaxPeople = trip.MaxPeople,
            Countries = trip.IdCountries.Select(country => country.MapToCountryDto()).ToList(),
            Clients = trip.ClientTrips.Select(e => e.IdClientNavigation.MapToCountryDto()).ToList()
        };
    }
}