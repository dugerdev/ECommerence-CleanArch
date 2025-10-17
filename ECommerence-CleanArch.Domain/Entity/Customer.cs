using ECommerence_CleanArch.Domain.Common;

namespace ECommerence_CleanArch.Domain.Entity;

// Müşteri bilgilerini tutan domain entity
// E-ticaret sisteminde alışveriş yapan kullanıcıları temsil eder
public class Customer : EntityBase
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // Kişisel Bilgiler
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    
    // Müşterinin adı
    public string FirstName { get; set; } = string.Empty;
    
    // Müşterinin soyadı
    public string LastName { get; set; } = string.Empty;
    
    // Email adresi (Benzersiz olmalı - unique constraint)
    public string Email { get; set; } = string.Empty;
    
    // Telefon numarası
    public string PhoneNumber { get; set; } = string.Empty;
    
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // Adres Bilgileri
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    
    // Ülke
    public string Country { get; set; } = string.Empty;
    
    // Şehir
    public string City { get; set; } = string.Empty;
    
    // Açık adres
    public string Address { get; set; } = string.Empty;
    
    // Posta kodu
    public string PostalCode { get; set; } = string.Empty;
    
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // Navigation Properties (İlişkiler)
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    
    // Müşterinin geçmiş siparişleri
    // One-to-Many: Bir müşterinin birden fazla siparişi olabilir
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    
    // Müşterinin alışveriş sepetleri (aktif ve geçmiş)
    // One-to-Many: Bir müşterinin birden fazla sepeti olabilir
    public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
