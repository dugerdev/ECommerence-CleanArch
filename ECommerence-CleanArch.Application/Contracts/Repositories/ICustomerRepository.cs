using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Contracts.Repositories;

/// <summary>
/// Customer-specific repository interface
/// Generic base + özel metodlar
/// </summary>
public interface ICustomerRepository : IAsyncRepository<Customer>
{
    /// <summary>
    /// Email'e göre müşteri getir
    /// </summary>
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Aktif müşterileri getir
    /// </summary>
    Task<IEnumerable<Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Şehre göre müşterileri getir
    /// </summary>
    Task<IEnumerable<Customer>> GetCustomersByCityAsync(string city, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Telefon numarasına göre müşteri getir
    /// </summary>
    Task<Customer?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Aktif müşterileri sayfalanmış olarak getir
    /// </summary>
    Task<Paginate<Customer>> GetPagedActiveCustomersAsync(
        int index = 0, 
        int size = 10, 
        CancellationToken cancellationToken = default);
}