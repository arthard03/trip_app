namespace trip_app.Exceptions;

public abstract class NotFoundException(string message) : Exception(message);
 
 public class ClintTripException(int id ) : NotFoundException($"Client: {id} cannot be deleted as they have assigned trips");

 public class TripDoesNotExsits(int id) : NotFoundException($"Trip:  {id} does not exist or has already started");

 public class ClientAlreadyRegistered(int id) : NotFoundException($"Client: {id} is already registered for this trip");
 public class TripAlreadyStartedException (int id): NotFoundException($"Trip: {id} has already started or completed");