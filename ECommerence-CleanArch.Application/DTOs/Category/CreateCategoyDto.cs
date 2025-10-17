namespace ECommerence_CleanArch.Application.DTOs.Category;

public class CreateCategoyDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid ParentCategoyId { get; set; }
}
