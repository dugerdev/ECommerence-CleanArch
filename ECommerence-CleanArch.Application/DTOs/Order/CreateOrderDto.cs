namespace ECommerence_CleanArch.Application.DTOs.Order;

/// <summary>
/// Yeni sipariş oluşturmak için
/// POST /api/orders endpoint'i için
/// </summary>
public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    
    // Sipariş kalemleri (sepetteki ürünlerden gelir)
    public List<CreateOrderItemDto> Items { get; set; } = new();
    
    // Teslimat adresi bilgileri
    public string ShippingAddress { get; set; } = string.Empty;
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingCountry { get; set; } = string.Empty;
    public string ShippingPostalCode { get; set; } = string.Empty;
    
    // NOT: OrderNumber yok (otomatik oluşturulur: ORD-20240115-001)
    // NOT: OrderDate yok (otomatik: DateTime.UtcNow)
    // NOT: TotalAmount yok (items'lardan hesaplanır)
    // NOT: OrderStatus yok (default: Pending)
    // NOT: PaymentStatus yok (default: CreditCard veya kullanıcıdan alınır)
}


