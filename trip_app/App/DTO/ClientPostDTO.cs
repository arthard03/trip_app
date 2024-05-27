namespace trip_app.DTO;

public class ClientPostDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string  LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Telephone { get; set; } =string.Empty;

    public string Pesel { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
};