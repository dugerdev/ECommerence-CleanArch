namespace ECommerence_CleanArch.Domain.Common;

// Tüm entity'ler için ortak özellikleri içeren temel sınıf
// Audit alanları ve soft delete işlevselliği sağlar
public abstract class EntityBase
{
    // Benzersiz tanımlayıcı (Primary Key)
    public Guid Id { get; set; }
    
    // Varlığın sistemde aktif olup olmadığını belirtir
    // İş mantığında pasif kayıtlar filtrelenebilir
    public bool IsActive { get; set; } = true;
    
    // Soft delete bayrağı - true olduğunda kayıt silinmiş sayılır
    // Veritabanında kalır, global sorgu filtresi otomatik uygular
    public bool IsDeleted { get; set; } = false;
    
    // Kaydın oluşturulma zamanı (UTC)
    // Repository tarafından insert sırasında otomatik atanır
    public DateTimeOffset CreatedAt { get; set; }
    
    // Kaydın son güncellenme zamanı (UTC)
    // Repository tarafından update sırasında otomatik atanır
    // Hiç güncellenmemişse null
    public DateTimeOffset? UpdatedAt { get; set; }
    
    // Kaydın silinme zamanı (UTC)
    // Sadece IsDeleted = true olduğunda dolu
    // Silinmemişse null
    public DateTimeOffset? DeletedAt { get; set; }
}
