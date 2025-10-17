using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Contracts.Repositories;

/// <summary>
/// Order-specific repository interface
/// Generic base + özel metodlar
/// </summary>
public interface IOrderRepository : IAsyncRepository<Order>
{
    /// <summary>
    /// Müşteri ID'sine göre siparişleri getir
    /// </summary>
    Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sipariş durumuna göre siparişleri getir
    /// </summary>
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Tarih aralığına göre siparişleri getir
    /// </summary>
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sipariş numarasına göre sipariş getir
    /// </summary>
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Ödeme durumuna göre siparişleri getir
    /// </summary>
    Task<IEnumerable<Order>> GetOrdersByPaymentStatusAsync(PaymentStatus paymentStatus, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Müşteriye göre sayfalanmış siparişleri getir
    /// </summary>
    Task<Paginate<Order>> GetPagedOrdersByCustomerAsync(
        Guid customerId, 
        int index = 0, 
        int size = 10, 
        CancellationToken cancellationToken = default);
}