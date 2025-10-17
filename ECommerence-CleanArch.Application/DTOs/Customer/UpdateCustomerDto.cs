namespace ECommerence_CleanArch.Application.DTOs.Customer;

/// <summary>
/// Müşteri bilgilerini güncellemek için
/// PUT /api/customers/{id} endpoint'i için
/// </summary>
public class UpdateCustomerDto
{
    public Guid Id { get; set; } // Hangi müşteri güncellenecek
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    // Adres bilgileri
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    
    // NOT: Email yok (değiştirilemez - güvenlik nedeniyle)
    // Email değiştirmek için ayrı endpoint olmalı (ConfirmEmail ile)
}


