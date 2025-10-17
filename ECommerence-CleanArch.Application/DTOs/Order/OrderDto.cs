namespace ECommerence_CleanArch.Application.DTOs.Order;

/// <summary>
/// Sipariş bilgilerini döndürmek için
/// GET /api/orders endpoint'i için
/// </summary>
public class OrderDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    
    // Enum'lar string olarak
    public string OrderStatus { get; set; } = string.Empty; // Pending, Processing, Shipped, vb.
    public string PaymentStatus { get; set; } = string.Empty; // CreditCard, Cash, vb.
    
    // Müşteri bilgisi (Navigation property yerine flat)
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    
    // Teslimat adresi
    public string ShippingAddress { get; set; } = string.Empty;
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingCountry { get; set; } = string.Empty;
    public string ShippingPostalCode { get; set; } = string.Empty;
    
    // Sipariş kalemleri (Navigation property yerine DTO listesi)
    public List<OrderItemDto> OrderItems { get; set; } = new();
    
    // Audit bilgileri
    public DateTimeOffset CreatedAt { get; set; }
}


