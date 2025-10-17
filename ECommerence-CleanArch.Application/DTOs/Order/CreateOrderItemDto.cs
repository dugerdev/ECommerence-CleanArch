namespace ECommerence_CleanArch.Application.DTOs.Order;

/// <summary>
/// Sipariş kalemi oluşturmak için
/// CreateOrderDto içinde kullanılır
/// </summary>
public class CreateOrderItemDto
{
    public Guid ProductId { get; set; } // Hangi ürün
    public int Quantity { get; set; }    // Kaç adet
    
    // NOT: UnitPrice yok (DB'den product.Price çekilir - güvenlik!)
    // Kullanıcı fiyatı manipüle edemez
    
    // NOT: ProductName yok (DB'den product.Name çekilir - snapshot)
    
    // NOT: TotalPrice yok (UnitPrice * Quantity ile hesaplanır)
}


