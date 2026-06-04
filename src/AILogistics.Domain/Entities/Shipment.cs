using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Domain.Entities
{
    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        Failed = 2
    }

    public enum ShipmentStatus
    {
        Pending = 0,
        InTransit = 1,
        Delivered = 2,
        Cancelled = 3
    }

    public enum ShipmentPriority 
    { 
        Normal = 0,
        High = 1,
        Urgent = 2
    }

    public class Shipment : BaseEntity
    {

        public string ShipmentNumber { get; set; }
        public int CustomerId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }

        public decimal WeightKg { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ShipmentStatus Status { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public ShipmentPriority Priority { get; set; }
        public decimal DistanceKm { get; set; }

        public Customer Customer { get; set; } = null!;

    }
}
