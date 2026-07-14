using AILogistics.Application.Customers;
using AILogistics.Application.Exceptions;
using AILogistics.Application.Interface;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.Services
{
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
            Customer customer = new Customer();
            customer.CompanyName = request.CompanyName;
            customer.ContactPerson = request.ContactPerson;
            customer.Email = request.Email;
            customer.PhoneNumber = request.PhoneNumber;
            customer.Address = request.Address;
            customer.City = request.City;
            customer.IsActive = true;
            customer.CreatedAt = DateTime.UtcNow;
            customer.UpdatedAt = DateTime.UtcNow;
            customer.State = request.State;
            customer.Country = request.Country;
            customer.PostalCode = request.PostalCode;

            await _context.Customers.AddAsync(customer);

            await _context.SaveChangesAsync();

            CustomerResponse response = new CustomerResponse();
            response.Id = customer.Id;
            response.CompanyName = customer.CompanyName;
            response.ContactPerson = customer.ContactPerson;
            response.Email = customer.Email;
            response.PhoneNumber = customer.PhoneNumber;
            response.Address = customer.Address;
            response.City = customer.City;
            response.State = customer.State;
            response.Country = customer.Country;
            response.PostalCode = customer.PostalCode;
            response.IsActive = customer.IsActive;
            response.CreatedAt = customer.CreatedAt;
            response.UpdatedAt = customer.UpdatedAt;

            return response;
        }

        public async Task<CustomerResponse?> GetCustomerById(int customerId)
        {
            Customer? customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == customerId);

            if (customer != null)
            {
                CustomerResponse response = new CustomerResponse
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

                };
                return response;
            }
            else
            {
                throw new NotFoundException("No customer found");
            }

        }

        public async Task<List<CustomerResponse>> GetCustomers()
        {
            _logger.LogInformation(
                "Fetching customers from database at {Time}",
                DateTime.UtcNow);

            List<Customer> customers = await _context.Customers.ToListAsync();

            List<CustomerResponse> customerList = new List<CustomerResponse>();

            foreach (var cs in customers)
            {
                CustomerResponse customer = new CustomerResponse
                {
                    Id = cs.Id,
                    CompanyName = cs.CompanyName,
                    ContactPerson = cs.ContactPerson,
                    Email = cs.Email,
                    PhoneNumber = cs.PhoneNumber,
                    Address = cs.Address,
                    City = cs.City,
                    State = cs.State,
                    Country = cs.Country,
                    PostalCode = cs.PostalCode,
                    IsActive = cs.IsActive,
                    CreatedAt = cs.CreatedAt,
                    UpdatedAt = cs.UpdatedAt,
                };
                customerList.Add(customer);
            }

            return customerList;

        }

        public async Task<CustomerResponse?> UpdateCustomer(CreateCustomerRequest request, int id)
        {
            Customer? customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customer != null)
            {
                customer.CompanyName = request.CompanyName;
                customer.ContactPerson = request.ContactPerson;
                customer.Email = request.Email;
                customer.PhoneNumber = request.PhoneNumber;
                customer.Address = request.Address;
                customer.City = request.City;
                customer.State = request.State;
                customer.Country = request.Country;
                customer.PostalCode = request.PostalCode;
                customer.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                CustomerResponse customerResponse = new CustomerResponse
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
                    UpdatedAt = customer.UpdatedAt,
                    CreatedAt = customer.CreatedAt,
                    IsActive = customer.IsActive,

                };

                return customerResponse;

            }

            else
            {
                return null;
            }

        }


        public async Task<bool> DeleteCustomer(int customerId)
        {
            Customer? deleteCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == customerId);
            if (deleteCustomer != null)
            {
                //_context.Customers.Remove(deleteCustomer);
                deleteCustomer.IsActive = false;
                deleteCustomer.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
