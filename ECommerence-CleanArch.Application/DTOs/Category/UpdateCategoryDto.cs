namespace ECommerence_CleanArch.Application.DTOs.Category;

public class UpdateCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid ParentCategoryId { get; set; }
    public bool IsActive { get; set; }
}
