using AILogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.DTOs.Shipments
{
    public class ShipmentResponse
    {
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal WeightKg { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
