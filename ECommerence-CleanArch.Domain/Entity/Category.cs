using ECommerence_CleanArch.Domain.Common;

namespace ECommerence_CleanArch.Domain.Entity;

// Ürün kategorilerini tutan domain entity
// Hiyerarşik yapı destekler (Ana kategori - Alt kategori ilişkisi)
public class Category : EntityBase
{
    // Kategori adı (zorunlu)
    public string Name { get; set; } = string.Empty;
    
    // Kategori açıklaması
    public string Description { get; set; } = string.Empty;
    
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // Hierarchical (Hiyerarşik) İlişki
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    
    // Üst kategori ID'si (Foreign Key - Self Referencing)
    // Null ise bu kategori ana kategoridir (root)
    public Guid? ParentCategoryId { get; set; }
    
    // Üst kategori (Parent category navigation property)
    // Örn: "Elektronik" kategorisinin altındaki "Telefon" için üst kategori "Elektronik"
    public Category? ParentCategory { get; set; }
    
    // Alt kategoriler (Child categories collection)
    // Örn: "Elektronik" kategorisi için ["Telefon", "Bilgisayar", "TV"] alt kategoriler
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // One-to-Many İlişkisi (Kategori → Ürünler)
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    
    // Bu kategoriye ait tüm ürünler
    // Örn: "Telefon" kategorisi için [iPhone 15, Samsung S24, ...]
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
