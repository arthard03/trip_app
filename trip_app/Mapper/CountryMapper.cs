

using trip_app.DTO;
using trip_app.OutputFolder;

namespace TripApp.Application.Mappers;

public static class CountryMapper
{
    public static CountryDTO MapToCountryDto(this Country country)
    {
        return new CountryDTO
        {
            Name = country.Name
        };
    }
}