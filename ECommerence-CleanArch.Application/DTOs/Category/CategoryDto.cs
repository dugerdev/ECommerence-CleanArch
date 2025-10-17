namespace ECommerence_CleanArch.Application.DTOs.Category;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty ;
    public bool IsActive { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public int SubCategoryCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; } 
}
