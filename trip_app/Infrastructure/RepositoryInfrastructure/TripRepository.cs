using Microsoft.EntityFrameworkCore;
using trip_app.Exceptions;
using trip_app.Repository;

namespace trip_app.OutputFolder;

public class TripRepository : ITripRepository
{
    private readonly ApbdContext _tripDbContext;

    public TripRepository(ApbdContext tripDbContext)
    {
        _tripDbContext = tripDbContext;
    }

    public async Task<PaginatedResult<Trip>> GetPaginatedTripsAsync(int page = 1, int pageSize = 10)
    {
        var tripsQuery = _tripDbContext.Trips
            .Include(e => e.ClientTrips).ThenInclude(e => e.IdClientNavigation)
            .Include(e => e.IdCountries)
            .OrderBy(e => e.DateFrom);

        var tripsCount = await tripsQuery.CountAsync();
        var totalPages = tripsCount / pageSize;
        var trips = await _tripDbContext.Trips
            .Include(e => e.ClientTrips).ThenInclude(e => e.IdClientNavigation)
            .Include(e => e.IdCountries)
            .OrderBy(e => e.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<Trip>
        {
            PageSize = pageSize,
            PageNum = page,
            AllPages = totalPages,
            Data = trips
        };
    }

    public async Task<List<Trip>> GetAllTripsAsync()
    {
        return await _tripDbContext.Trips
            .Include(e => e.ClientTrips).ThenInclude(e => e.IdClientNavigation)
            .Include(e => e.IdCountries)
            .OrderByDescending(e => e.DateFrom)
            .ToListAsync();
    }
    
    public async Task<bool> DeleteClientAsync(int idClient)
    {
        var client = await _tripDbContext.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
        {
            return false;
        }

        if (client.ClientTrips.Any())
        {
            throw new ClintTripException(idClient);
        }

        _tripDbContext.Clients.Remove(client);
        await _tripDbContext.SaveChangesAsync();

        return true;
    }
    public async Task<Trip> GetTripByIdAsync(int idTrip)
    {
        return await _tripDbContext.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
    }

    public async Task<Client> GetClientByPeselAsync(string pesel)
    {
        return await _tripDbContext.Clients.FirstOrDefaultAsync(c => c.Pesel == pesel);
    }

    public async Task<bool> IsClientRegisteredForTripAsync(int idClient, int idTrip)
    {
        return await _tripDbContext.ClientTrips.AnyAsync(ct => ct.IdClient == idClient && ct.IdTrip == idTrip);
    }

    public async Task AddClientAsync(Client client)
    {
        _tripDbContext.Clients.Add(client);
        await _tripDbContext.SaveChangesAsync();
    }

    public async Task AddClientTripAsync(ClientTrip clientTrip)
    {
        _tripDbContext.ClientTrips.Add(clientTrip);
        await _tripDbContext.SaveChangesAsync();
    }
    
}