using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Configuration;

// DEĞIŞIKLIK: BaseEntityConfiguration'dan türetildi
public class CategoryConfiguration : BaseEntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        // ⭐ Base configuration (Id, CreatedAt, vb.)
        base.Configure(builder);

        // Table name
        builder.ToTable("Categories");

        // Properties
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        // Index
        builder.HasIndex(c => c.Name)
            .IsUnique()
            .HasDatabaseName("IX_Categories_Name");

        // Self-referencing relationship (Parent-Child)
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
