namespace ECommerence_CleanArch.Application.DTOs.Category;

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid ParentCategoryId { get; set; }
}
