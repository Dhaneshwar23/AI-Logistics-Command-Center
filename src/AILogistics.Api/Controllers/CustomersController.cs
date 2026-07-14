using AILogistics.Api.Extensions;
using AILogistics.Api.Filters;
using AILogistics.Application.Customers;
using AILogistics.Application.Interface;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;

namespace AILogistics.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        private readonly IOutputCacheStore _outputCacheStore;

        public CustomersController(ICustomerService customerService, IOutputCacheStore outputCacheStore)
        {
            _customerService = customerService;
            _outputCacheStore = outputCacheStore;
        }

        [HttpPost]
        [InvalidateOutputCache(OutputCachingExtensions.CustomersTag)]
        public async Task<IActionResult> CreateCustomer(CreateCustomerRequest customer)
        {

            var res = await _customerService.CreateCustomer(customer);

            return Ok(res);

        }

        [HttpGet("{customerId}")]
        [OutputCache(PolicyName = OutputCachingExtensions.GeneralPolicy,
            Tags = new[] { OutputCachingExtensions.CustomersTag })]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            var customer = await _customerService.GetCustomerById(customerId);

            if (customer == null || customer.IsActive == false)
            {
                return NotFound();

            }
            else
            {
                return Ok(customer);
            }

        }

        [HttpGet]
        [OutputCache(PolicyName = OutputCachingExtensions.GeneralPolicy,
            Tags = new[] {OutputCachingExtensions.CustomersTag})]
        public async Task<IActionResult> GetCustomers()
        {
            List<CustomerResponse> customers = await _customerService.GetCustomers();

            return Ok(customers);
        }

        [HttpPut("{customerId}")]
        [InvalidateOutputCache(OutputCachingExtensions.CustomersTag)]
        public async Task<IActionResult> UpdateCustomer(CreateCustomerRequest request, int customerId)
        {
            if (request == null)
            {
                return BadRequest();
            }
            else
            {
                CustomerResponse res = await _customerService.UpdateCustomer(request, customerId);
                if(res == null)
                {
                    return NotFound();
                }

                return Ok(res);
            }
        }

        [HttpDelete("{customerId}")]
        [InvalidateOutputCache(OutputCachingExtensions.CustomersTag)]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            if (customerId <= 0)
            {
                return BadRequest();
            }
            else
            {
                var res = await _customerService.DeleteCustomer(customerId);

                if (res == true)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
