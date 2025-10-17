using ECommerence_CleanArch.Domain.Common;

namespace ECommerence_CleanArch.Domain.Entity;

public class ShoppingCart : EntityBase
{
    public Guid CustomerId { get; set; } // DÜZELTME: int → Guid

    public Customer? Customer { get; set; }
   
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
