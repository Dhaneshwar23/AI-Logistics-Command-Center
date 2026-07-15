using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AILogistics.Application.Customers
{
    public class CreateCustomerRequest
    {
        
        [Required, StringLength(200)]
        public string CompanyName { get; set; }
        [Required, StringLength(200)]
        public string ContactPerson {  get; set; }
        [EmailAddress, StringLength(320)]
        public string? Email { get; set; }
        [Required, Phone, StringLength(30)]
        public string PhoneNumber { get; set; }
        [Required, StringLength(500)]
        public string Address { get; set; }
        [Required, StringLength(100)]
        public string City { get; set; }
        [Required, StringLength(100)]
        public string State {  get; set; }
        [Required, StringLength(100)]
        public string Country { get; set; }
        [Required, StringLength(20)]
        public string PostalCode { get; set; }
    }
}
