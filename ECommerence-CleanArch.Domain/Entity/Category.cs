using ECommerence_CleanArch.Domain.Common;

namespace ECommerence_CleanArch.Domain.Entity;

public class Category : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; } // DÜZELTME: int? → Guid?
    // IsActive EntityBase'de var, burada tekrar yazmaya gerek yok
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
}
