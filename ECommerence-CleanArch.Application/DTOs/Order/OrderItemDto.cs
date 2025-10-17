namespace ECommerence_CleanArch.Application.DTOs.Order;

/// <summary>
/// Sipariş kalemi bilgisi
/// OrderDto içinde kullanılır (nested DTO)
/// </summary>
public class OrderItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty; // Ürün adı (snapshot)
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; } // Quantity * UnitPrice
    
    // NOT: Order navigation property yok (circular reference engellendi)
}


