using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerence_CleanArch.Domain.Interfaces;
using ECommerence_CleanArch.Infrastructure.Persistance.Context;
using ECommerence_CleanArch.Infrastructure.Persistance.Repositories;
using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.Contracts.Repositories;

namespace ECommerence_CleanArch.Infrastructure;

/// <summary>
/// Infrastructure katmanının servislerini DI Container'a kaydeder
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Infrastructure servislerini ekle (DbContext, Repository'ler, UnitOfWork)
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // 1. DATABASE CONTEXT
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            )
        );
        
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // 2. GENERIC REPOSITORY REGISTRATIONS (YENİ)
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        
        services.AddScoped(typeof(IAsyncRepository<>), typeof(EFAsyncRepositoryBase<,>));
        
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // 3. SPECIFIC REPOSITORY REGISTRATIONS (YENİ)
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        
        // Yeni generic-based repository'ler (şimdilik generic repository kullanacağız)
        // services.AddScoped<IProductRepository, ProductRepository>();
        // services.AddScoped<ICategoryRepository, CategoryRepository>();
        // services.AddScoped<ICustomerRepository, CustomerRepository>();
        // services.AddScoped<IOrderRepository, OrderRepository>();
        
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // 4. LEGACY REPOSITORY REGISTRATIONS (MEVCUT)
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        
        // Eski repository'ler (şimdilik kalsın, sonra kaldıracağız)
        services.AddScoped<ECommerence_CleanArch.Domain.Interfaces.IProductRepository, 
                          ECommerence_CleanArch.Infrastructure.Persistance.Repositories.ProductRepository>();
        services.AddScoped<ECommerence_CleanArch.Domain.Interfaces.ICategoryRepository, 
                          ECommerence_CleanArch.Infrastructure.Persistance.Repositories.CategoryRepository>();
        services.AddScoped<ECommerence_CleanArch.Domain.Interfaces.ICustomerRepository, 
                          ECommerence_CleanArch.Infrastructure.Persistance.Repositories.CustomerRepository>();
        services.AddScoped<ECommerence_CleanArch.Domain.Interfaces.IOrderRepository, 
                          ECommerence_CleanArch.Infrastructure.Persistance.Repositories.OrderRepository>();
        services.AddScoped<ECommerence_CleanArch.Domain.Interfaces.IOrderItemRepository, 
                          ECommerence_CleanArch.Infrastructure.Persistance.Repositories.OrderItemRepository>();
        services.AddScoped<ECommerence_CleanArch.Domain.Interfaces.IShoppingCartRepository, 
                          ECommerence_CleanArch.Infrastructure.Persistance.Repositories.ShoppingCartRepository>();
        services.AddScoped<ECommerence_CleanArch.Domain.Interfaces.ICartItemRepository, 
                          ECommerence_CleanArch.Infrastructure.Persistance.Repositories.CartItemRepository>();
        
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // 3. UNIT OF WORK REGISTRATION
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}
