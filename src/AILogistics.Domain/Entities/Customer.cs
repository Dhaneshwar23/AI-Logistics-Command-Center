using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string? Email {  get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State {  get; set; }

        public string Country { get; set; }
        public string PostalCode { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();


    }
}
