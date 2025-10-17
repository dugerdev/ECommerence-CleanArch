namespace ECommerence_CleanArch.Application.DTOs.Customer;

/// <summary>
/// Müşteri bilgilerini API'ye döndürmek için kullanılır
/// GET /api/customers endpoint'i için
/// </summary>
public class CustomerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    // Computed property (hesaplanan alan)
    public string FullName => $"{FirstName} {LastName}";
    
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    // Adres bilgileri
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    
    // Audit bilgileri
    public DateTimeOffset CreatedAt { get; set; }
}


