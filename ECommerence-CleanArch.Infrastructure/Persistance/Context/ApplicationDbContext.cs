using Microsoft.EntityFrameworkCore;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Common;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Context;

// Ana veritabanı context sınıfı
// EF Core ile SQL Server arasındaki köprü görevi görür
public class ApplicationDbContext : DbContext
{
    // Constructor - Dependency Injection ile DbContextOptions alır
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // DbSet'ler - Her entity için veritabanı tablosu
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    
    public DbSet<Product> Products { get; set; }           // Ürünler tablosu
    public DbSet<Category> Categories { get; set; }        // Kategoriler tablosu
    public DbSet<Customer> Customers { get; set; }         // Müşteriler tablosu
    public DbSet<Order> Orders { get; set; }               // Siparişler tablosu
    public DbSet<OrderItem> OrderItems { get; set; }       // Sipariş kalemleri tablosu
    public DbSet<ShoppingCart> ShoppingCarts { get; set; } // Alışveriş sepetleri tablosu
    public DbSet<CartItem> CartItems { get; set; }         // Sepet kalemleri tablosu

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // Model Oluşturma (Entity Configuration)
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Assembly'deki tüm IEntityTypeConfiguration implementasyonlarını otomatik uygula
        // Örn: ProductConfiguration, CategoryConfiguration, vb.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Global Query Filters (Soft Delete için)
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Tüm sorgularda IsDeleted = false olanları otomatik filtrele
        // .IgnoreQueryFilters() ile devre dışı bırakılabilir
        
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
        modelBuilder.Entity<OrderItem>().HasQueryFilter(oi => !oi.IsDeleted);
        modelBuilder.Entity<ShoppingCart>().HasQueryFilter(sc => !sc.IsDeleted);
        modelBuilder.Entity<CartItem>().HasQueryFilter(ci => !ci.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // SaveChanges Override (Audit Alanlarını Otomatik Yönetir)
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // EntityBase'den türeyen tüm değişiklikleri al
        var entries = ChangeTracker.Entries<EntityBase>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // Yeni kayıt ekleniyorsa
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;  // Oluşturma zamanı
                    entry.Entity.IsActive = true;                    // Aktif olarak işaretle
                    entry.Entity.IsDeleted = false;                  // Silinmemiş olarak işaretle
                    break;

                case EntityState.Modified:
                    // Kayıt güncelleniyorsa
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;  // Güncelleme zamanı
                    break;

                case EntityState.Deleted:
                    // Kayıt siliniyorsa (Soft Delete)
                    entry.State = EntityState.Modified;              // Silme yerine güncelleme yap
                    entry.Entity.IsDeleted = true;                   // Silinmiş olarak işaretle
                    entry.Entity.DeletedAt = DateTimeOffset.UtcNow;  // Silinme zamanı
                    break;
            }
        }

        // Değişiklikleri veritabanına kaydet
        return await base.SaveChangesAsync(cancellationToken);
    }
}
