using ECommerence_CleanArch.Domain.Common;
using ECommerence_CleanArch.Domain.Common.Enums;

// HATA: using System.Security.Cryptography; → Gereksiz using, neden eklendi? 😄

namespace ECommerence_CleanArch.Domain.Entity;

public class Order : EntityBase
{
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public Guid CustomerId { get; set; } // DÜZELTME: int → Guid (Primary Key ile uyumlu)
    public string ShippingAddress { get; set; } = string.Empty;
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingCountry { get; set; } = string.Empty;
    public string ShippingPostalCode { get; set; } = string.Empty;
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.CreditCard;
    public Customer? Customer { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
