using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Configuration;

// DEĞIŞIKLIK: BaseEntityConfiguration'dan türetildi
public class CartItemConfiguration : BaseEntityConfiguration<CartItem>
{
    public override void Configure(EntityTypeBuilder<CartItem> builder)
    {
        // ⭐ Base configuration
        base.Configure(builder);

        // Table name
        builder.ToTable("CartItems");

        // Properties
        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.Property(ci => ci.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        // Index
        builder.HasIndex(ci => ci.ShoppingCartId)
            .HasDatabaseName("IX_CartItems_ShoppingCartId");

        builder.HasIndex(ci => ci.ProductId)
            .HasDatabaseName("IX_CartItems_ProductId");

        // Composite Index (Aynı sepette aynı ürün bir kez olmalı)
        builder.HasIndex(ci => new { ci.ShoppingCartId, ci.ProductId })
            .IsUnique()
            .HasDatabaseName("IX_CartItems_ShoppingCartId_ProductId");

        // Relationships
        builder.HasOne(ci => ci.ShoppingCart)
            .WithMany(sc => sc.CartItems)
            .HasForeignKey(ci => ci.ShoppingCartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
