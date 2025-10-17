using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Configuration;

// DEĞIŞIKLIK: BaseEntityConfiguration'dan türetildi
public class ShoppingCartConfiguration : BaseEntityConfiguration<ShoppingCart>
{
    public override void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        // ⭐ Base configuration
        base.Configure(builder);

        // Table name
        builder.ToTable("ShoppingCarts");

        // Index
        builder.HasIndex(sc => sc.CustomerId)
            .HasDatabaseName("IX_ShoppingCarts_CustomerId");

        // Relationships
        builder.HasOne(sc => sc.Customer)
            .WithMany(c => c.ShoppingCarts)
            .HasForeignKey(sc => sc.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
