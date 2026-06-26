using AILogistics.Application.Customers;
using AILogistics.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AILogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;

        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerRequest customer)
        {

            var res = await _customerService.CreateCustomer(customer);
            return Ok(res);

        }

        [HttpGet("{customerId}")]
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
        public async Task<IActionResult> GetCustomers()
        {
            List<CustomerResponse> customers = await _customerService.GetCustomers();

            return Ok(customers);
        }

        [HttpPut("{customerId}")]
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
