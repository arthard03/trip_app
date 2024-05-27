namespace trip_app.Exceptions;

public abstract class NotFoundException(string message) : Exception(message);
 
 public class ClintTripException(int id ) : NotFoundException($"Client: {id} cannot be deleted as they have assigned trips");

 public class TripDoesNotExsits(int id) : NotFoundException($"Trip:  {id} does not exist");

 public class ClientAlreadyRegistered(int id,string pesel) : NotFoundException($"Client: {id} is already registered for this trip with pesel : {pesel}");
 public class TripAlreadyStartedException (int id): NotFoundException($"Trip: {id} has already started or completed");