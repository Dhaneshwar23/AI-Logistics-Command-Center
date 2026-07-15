using AILogistics.Application.Common;
using AILogistics.Application.Customers;

namespace AILogistics.Application.Interface;

public interface ICustomerService
{
    Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request);
    Task<PagedResponse<CustomerResponse>> GetCustomers(PaginationRequest pagination, CancellationToken cancellationToken = default);
    Task<CustomerResponse?> GetCustomerById(int customerId);
    Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, int id);
    Task<bool> DeleteCustomer(int customerId);
}
