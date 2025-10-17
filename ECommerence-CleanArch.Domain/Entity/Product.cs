using ECommerence_CleanArch.Domain.Common;
using ECommerence_CleanArch.Domain.Common.Enums;

namespace ECommerence_CleanArch.Domain.Entity;

public class Product : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Currency PriceCurrency {  get; set; }
    public int Stock { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    
    // IsActive EntityBase'de var, burada tekrar yazmaya gerek yok

    public Guid CategoryId { get; set; } // DÜZELTME: int → Guid
    public Category? Category { get; set; }
}
