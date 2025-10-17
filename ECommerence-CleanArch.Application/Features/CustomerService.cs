using AutoMapper;
using ECommerence_CleanArch.Application.Contracts.Repositories;
using ECommerence_CleanArch.Application.Contracts.Services;
using ECommerence_CleanArch.Application.DTOs.Customer;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Features;

/// <summary>
/// Customer service implementation
/// Business logic + Repository coordination
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<Paginate<CustomerDto>> GetListAsync(
        Expression<Func<Customer, bool>>? predicate = null,
        Func<IQueryable<Customer>, IOrderedQueryable<Customer>>? orderBy = null,
        Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var pagedCustomers = await _customerRepository.GetListAsync(
            predicate, orderBy, include, index, size, withDeleted, enableTracking, cancellationToken);

        return new Paginate<CustomerDto>
        {
            Index = pagedCustomers.Index,
            Size = pagedCustomers.Size,
            Count = pagedCustomers.Count,
            Pages = pagedCustomers.Pages,
            Items = _mapper.Map<IList<CustomerDto>>(pagedCustomers.Items)
        };
    }

    public async Task<CustomerDto?> GetAsync(
        Expression<Func<Customer, bool>> predicate,
        Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<bool> AnyAsync(
        Expression<Func<Customer, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        return await _customerRepository.AnyAsync(predicate, withDeleted, enableTracking, cancellationToken);
    }

    public async Task<CustomerDto> AddAsync(Customer entity)
    {
        var addedCustomer = await _customerRepository.AddAsync(entity);
        return _mapper.Map<CustomerDto>(addedCustomer);
    }

    public async Task<CustomerDto> UpdateAsync(Customer entity)
    {
        var updatedCustomer = await _customerRepository.UpdateAsync(entity);
        return _mapper.Map<CustomerDto>(updatedCustomer);
    }

    public async Task<CustomerDto> DeleteAsync(Customer entity, bool permanent = false)
    {
        var deletedCustomer = await _customerRepository.DeleteAsync(entity, permanent);
        return _mapper.Map<CustomerDto>(deletedCustomer);
    }

    public async Task<CustomerDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByEmailAsync(email, cancellationToken);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<IEnumerable<CustomerDto>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetActiveCustomersAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomersByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetCustomersByCityAsync(city, cancellationToken);
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }
}
