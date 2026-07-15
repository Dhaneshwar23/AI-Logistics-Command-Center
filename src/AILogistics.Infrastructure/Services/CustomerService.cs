using AILogistics.Application.Common;
using AILogistics.Application.Customers;
using AILogistics.Application.Exceptions;
using AILogistics.Application.Interface;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AILogistics.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ApplicationDbContext context, ILogger<CustomerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            CompanyName = request.CompanyName,
            ContactPerson = request.ContactPerson,
            Email = request.Email?.Trim().ToLowerInvariant(),
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            City = request.City,
            State = request.State,
            Country = request.Country,
            PostalCode = request.PostalCode,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return Map(customer);
    }

    public async Task<CustomerResponse?> GetCustomerById(int customerId)
    {
        return await _context.Customers
            .AsNoTracking()
            .Where(customer => customer.Id == customerId && customer.IsActive)
            .Select(customer => Map(customer))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException("No customer found");
    }

    public async Task<PagedResponse<CustomerResponse>> GetCustomers(
        PaginationRequest pagination,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching customer page {PageNumber}", pagination.PageNumber);

        var query = _context.Customers.AsNoTracking().Where(customer => customer.IsActive);
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(customer => customer.Id)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(customer => Map(customer))
            .ToListAsync(cancellationToken);

        return new PagedResponse<CustomerResponse>(items, pagination.PageNumber, pagination.PageSize, totalCount);
    }

    public async Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, int id)
    {
        var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Id == id && x.IsActive)
            ?? throw new NotFoundException("No customer found");

        _context.Entry(customer).Property(x => x.RowVersion).OriginalValue = request.RowVersion;
        customer.CompanyName = request.CompanyName;
        customer.ContactPerson = request.ContactPerson;
        customer.Email = request.Email?.Trim().ToLowerInvariant();
        customer.PhoneNumber = request.PhoneNumber;
        customer.Address = request.Address;
        customer.City = request.City;
        customer.State = request.State;
        customer.Country = request.Country;
        customer.PostalCode = request.PostalCode;
        customer.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConcurrencyException("The customer was modified by another request. Reload it and retry.", exception);
        }

        return Map(customer);
    }

    public async Task<bool> DeleteCustomer(int customerId)
    {
        var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Id == customerId && x.IsActive);
        if (customer is null) return false;

        customer.IsActive = false;
        customer.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    private static CustomerResponse Map(Customer customer) => new()
    {
        Id = customer.Id,
        CompanyName = customer.CompanyName,
        ContactPerson = customer.ContactPerson,
        Email = customer.Email,
        PhoneNumber = customer.PhoneNumber,
        Address = customer.Address,
        City = customer.City,
        State = customer.State,
        Country = customer.Country,
        PostalCode = customer.PostalCode,
        IsActive = customer.IsActive,
        CreatedAt = customer.CreatedAt,
        UpdatedAt = customer.UpdatedAt,
        RowVersion = customer.RowVersion
    };
}
