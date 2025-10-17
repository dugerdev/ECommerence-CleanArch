using ECommerence_CleanArch.Domain.Common;
using ECommerence_CleanArch.Domain.Common.Enums;

namespace ECommerence_CleanArch.Domain.Entity;

// Ürün bilgilerini tutan domain entity
// E-ticaret sistemindeki satılabilir ürünleri temsil eder
public class Product : EntityBase
{
    // Ürün adı (zorunlu)
    public string Name { get; set; } = string.Empty;
    
    // Ürün açıklaması (detaylı bilgi)
    public string Description { get; set; } = string.Empty;
    
    // Ürün fiyatı (decimal - hassas hesaplama için)
    public decimal Price { get; set; }
    
    // Fiyat para birimi (USD, EUR, GBP, TRY)
    public Currency PriceCurrency { get; set; }
    
    // Stok miktarı (adet)
    public int Stock { get; set; }
    
    // Stok Takip Kodu (Stock Keeping Unit - Benzersiz olmalı)
    public string SKU { get; set; } = string.Empty;
    
    // Ürün görseli URL'i
    public string ImageUrl { get; set; } = string.Empty;
    
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // Navigation Properties (İlişkiler)
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    
    // Ürünün ait olduğu kategori ID'si (Foreign Key)
    public Guid CategoryId { get; set; }
    
    // Ürünün kategorisi (EF Core navigation property)
    // ? işareti: Kategori yüklenene kadar null olabilir (lazy loading)
    public Category? Category { get; set; }
}
