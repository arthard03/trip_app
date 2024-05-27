using Microsoft.VisualBasic;

namespace trip_app.DTO;

public class TripDTO
{
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public required DateTime DateFrom { get; set; }
    public required DateTime DateTo { get; set; }
    public required int MaxPeople { get; set; }
    public required List<CountryDTO> Countries { get; set; } = [];
    public required List<ClientDTO> Clients { get; set; } = [];
}