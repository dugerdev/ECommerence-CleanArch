using ECommerence_CleanArch.Domain.Common;

namespace ECommerence_CleanArch.Domain.Entity;

public class CartItem : EntityBase
{
    public Guid ShoppingCartId { get; set; } // DÜZELTME: int → Guid
    public Guid ProductId { get; set; } // DÜZELTME: int → Guid
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public ShoppingCart? ShoppingCart { get; set; }
    public Product? Product { get; set; }
}
