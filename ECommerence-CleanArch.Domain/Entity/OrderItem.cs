using ECommerence_CleanArch.Domain.Common;

namespace ECommerence_CleanArch.Domain.Entity;

public class OrderItem : EntityBase
{
    public Guid OrderId { get; set; } // DÜZELTME: int → Guid
    public Guid ProductId { get; set; } // DÜZELTME: int → Guid
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public Order? Order { get; set; }
    public Product? Product { get; set; }
}
