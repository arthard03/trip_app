

using trip_app.DTO;
using trip_app.OutputFolder;

namespace TripApp.Application.Mappers;

public static class ClientMapper
{
    public static ClientDTO MapToCountryDto(this Client client)
    {
        return new ClientDTO
        {
            FirstName = client.FirstName,
            LastName = client.LastName
        };
    }
}