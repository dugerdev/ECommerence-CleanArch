using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Common;

// Generic Repository Base Class - Tüm entity'ler için temel CRUD operasyonları
// TEntity: İşlem yapılacak entity tipi (EntityBase'den türemeli)
// TContext: Veritabanı context'i (DbContext'ten türemeli)
public class EFAsyncRepositoryBase<TEntity, TContext>(TContext context)
    : IAsyncRepository<TEntity>
    where TEntity : EntityBase
    where TContext : DbContext
{
    // EF Core DbContext - Veritabanı işlemleri için
    protected readonly TContext _context = context;

    // Entity'ye ait IQueryable döndürür - Sorgu oluşturmak için kullanılır
    public IQueryable<TEntity> Query() => _context.Set<TEntity>();

    // Yeni kayıt ekler (INSERT)
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        // Yeni Guid ID ata
        entity.Id = Guid.NewGuid();
        
        // Oluşturma zamanını UTC olarak kaydet
        entity.CreatedAt = DateTimeOffset.UtcNow;
        
        // Entity'yi context'e ekle
        await _context.AddAsync(entity);
        
        // Değişiklikleri veritabanına yaz
        await _context.SaveChangesAsync();
        
        return entity;
    }

    // Mevcut kaydı günceller (UPDATE)
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        // Güncellenme zamanını UTC olarak kaydet
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        
        // Entity'yi güncelle olarak işaretle
        _context.Update(entity);
        
        // Değişiklikleri veritabanına yaz
        await _context.SaveChangesAsync();
        
        return entity;
    }

    // Kaydı siler (Soft Delete veya Hard Delete)
    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
    {
        if (permanent)
        {
            // Hard Delete: Veritabanından tamamen sil
            _context.Remove(entity);
        }
        else
        {
            // Soft Delete: IsDeleted bayrağını true yap
            entity.IsDeleted = true;
            
            // Silinme zamanını UTC olarak kaydet
            entity.DeletedAt = DateTimeOffset.UtcNow;
            
            // Entity'yi güncelle
            _context.Update(entity);
        }

        // Değişiklikleri veritabanına yaz
        await _context.SaveChangesAsync();
        
        return entity;
    }

    // Belirli koşula uyan kayıt var mı kontrol eder
    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null, // Filtre koşulu (örn: x => x.Name == "Test")
        bool withDeleted = false, // Silinmiş kayıtları dahil et mi?
        bool enableTracking = false, // EF Core change tracking aktif olsun mu?
        CancellationToken cancellationToken = default // İptal token'ı
        )
    {
        IQueryable<TEntity> queryable = Query();

        // Tracking kapalıysa performans için AsNoTracking kullan
        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        // Silinmiş kayıtları da getir (global query filter'ı devre dışı bırak)
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();

        // Filtre varsa uygula
        if (predicate is not null)
            queryable = queryable.Where(predicate);

        return await queryable.AnyAsync(cancellationToken);
    }

    // Sayfalanmış liste getirir (Pagination)
    public async Task<Paginate<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null, // Filtre koşulu
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, // Sıralama (örn: x => x.OrderBy(p => p.Name))
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, // İlişkili tabloları yükle (Eager Loading)
        int index = 0, // Sayfa numarası (0'dan başlar)
        int size = 10, // Sayfa başına kayıt sayısı
        bool withDeleted = false, // Silinmiş kayıtları dahil et mi?
        bool enableTracking = false, // EF Core change tracking aktif olsun mu?
        CancellationToken cancellationToken = default // İptal token'ı
        )
    {
        IQueryable<TEntity> queryable = Query();

        // Tracking kapalıysa performans için AsNoTracking kullan (Read-only sorgular için önerilir)
        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        // İlişkili tabloları yükle (Eager Loading ile N+1 sorunundan kaçın)
        // Örn: include: x => x.Include(p => p.Category)
        if (include is not null)
            queryable = include(queryable);

        // Silinmiş kayıtları da getir (global query filter'ı devre dışı bırak)
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();

        // Filtre varsa uygula (örn: x => x.IsActive == true)
        if (predicate is not null)
            queryable = queryable.Where(predicate);

        // Sıralama varsa uygula ve sayfalandır
        if (orderBy is not null)
            return await orderBy(queryable).ToPaginateAsync(index, size, cancellationToken);

        // Sıralama yoksa doğrudan sayfalandır
        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    // Tekil kayıt getirir (Single record)
    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate, // Filtre koşulu (zorunlu)
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, // İlişkili tabloları yükle
        bool withDeleted = false, // Silinmiş kayıtları dahil et mi?
        bool enableTracking = false, // EF Core change tracking aktif olsun mu?
        CancellationToken cancellationToken = default // İptal token'ı
        )
    {
        IQueryable<TEntity> queryable = Query();

        // Tracking kapalıysa performans için AsNoTracking kullan
        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        // İlişkili tabloları yükle (Eager Loading)
        // Örn: include: x => x.Include(p => p.Category).ThenInclude(c => c.ParentCategory)
        if (include is not null)
            queryable = include(queryable);

        // Silinmiş kayıtları da getir (global query filter'ı devre dışı bırak)
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();

        // Tek kayıt döndür (bulunamazsa null, birden fazla bulursa exception)
        return await queryable.SingleOrDefaultAsync(predicate, cancellationToken);
    }
}