using AILogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.Customers
{
    public interface ICustomerService
    {
        public Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request);

        public Task<List<CustomerResponse>> GetCustomers();
        public Task<CustomerResponse?> GetCustomerById(int customerId);

        public Task<CustomerResponse> UpdateCustomer(CreateCustomerRequest request, int id);

        public Task<bool> DeleteCustomer(int customerId);
    }
}
