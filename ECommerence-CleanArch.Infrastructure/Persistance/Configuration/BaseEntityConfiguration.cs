using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerence_CleanArch.Domain.Common;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Configuration;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd(); // Guid otomatik oluştur

        // IsActive
        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true); // Default true

        // IsDeleted
        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false); // Default false

        // CreatedAt
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        // UpdatedAt (nullable)
        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);

        // DeletedAt (nullable)
        builder.Property(e => e.DeletedAt)
            .IsRequired(false);

        // Index for soft delete queries
        builder.HasIndex(e => e.IsDeleted)
            .HasDatabaseName($"IX_{typeof(T).Name}_IsDeleted");

        // Index for CreatedAt (sorting, filtering)
        builder.HasIndex(e => e.CreatedAt)
            .HasDatabaseName($"IX_{typeof(T).Name}_CreatedAt");
    }
}
