using trip_app.DTO;
using trip_app.Exceptions;
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
        var mappedTrips = trips.Select(trip => trip.MapToGetTripDto()).ToList();
        return mappedTrips;
    }

    public async Task<bool> DeleteClientAsync(int idClient)
    {
        try
        {
            var result = await _tripRepository.DeleteClientAsync(idClient);
            return result;
        }
        catch (NotFoundException ex)
        {

            throw new ClintTripException(idClient);
        }
    }
    
    public async Task<bool> AssignClientToTripAsync(ClientPostDTO clientDto, int idTrip)
    {
        var trip = await _tripRepository.GetTripByIdAsync(idTrip);
        if (trip == null)
        {
            throw new TripDoesNotExsits(idTrip); 
        }
        
        if (trip.DateFrom <= DateTime.Now)
        {
            throw new TripAlreadyStartedException(idTrip);  
        }


        var client = await _tripRepository.GetClientByPeselAsync(clientDto.Pesel);
        if (client != null)
        {
            var isClientRegistered = await _tripRepository.IsClientRegisteredForTripAsync(client.IdClient, idTrip);
            if (isClientRegistered)
            {
                throw new ClientAlreadyRegistered(client.IdClient,client.Pesel);
            }
        }
        else
        {
            client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                Telephone = clientDto.Telephone,
                Pesel = clientDto.Pesel
            };

            await _tripRepository.AddClientAsync(client);
        }

        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = DateTime.Now
        };

        await _tripRepository.AddClientTripAsync(clientTrip);

        return true;
    }
}