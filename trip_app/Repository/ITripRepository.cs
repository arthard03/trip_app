using trip_app.DTO;
using trip_app.OutputFolder;

namespace trip_app.Repository;

public interface ITripRepository
{
    Task<PaginatedResult<Trip>> GetPaginatedTripsAsync(int page = 1, int pageSize = 10);
    Task<List<Trip>> GetAllTripsAsync();
    Task<bool> DeleteClientAsync(int idClient);
    // Task<bool> AssignClientToTripAsync(ClientDTO clientDto, int idTrip);
    Task<Trip> GetTripByIdAsync(int idTrip);
    Task<Client> GetClientByPeselAsync(string pesel);
    Task<bool> IsClientRegisteredForTripAsync(int idClient, int idTrip);
    Task AddClientAsync(Client client);
    Task AddClientTripAsync(ClientTrip clientTrip);
    
}