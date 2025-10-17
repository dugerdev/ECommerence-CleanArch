namespace ECommerence_CleanArch.Application.DTOs.Customer;

/// <summary>
/// Yeni müşteri kaydı için kullanılır (Register)
/// POST /api/customers endpoint'i için
/// </summary>
public class CreateCustomerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    // Adres bilgileri
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    
    // NOT: Id yok (otomatik oluşur)
    // NOT: CreatedAt yok (otomatik set edilir)
}


