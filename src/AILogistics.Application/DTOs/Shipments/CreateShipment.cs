using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.DTOs.Shipments
{
    public class CreateShipment
    {
        public int CustomerId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal WeightKg { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
