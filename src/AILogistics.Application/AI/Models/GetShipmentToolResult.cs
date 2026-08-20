using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.AI.Models
{
    public sealed class GetShipmentToolResult
    {
        public int Id { get; init; }
        public string ShipmentNumber { get; init; } = string.Empty;
        public string CustomerName { get; init; } = string.Empty;
        public string Origin {  get; init; } = string.Empty;
        public string Destination { get; init; } = string.Empty;
        public decimal WeightKg { get; init; }
        public string ShipmentStatus { get; init; }
        public string PaymentStatus { get; init; }
        public DateTime? PickupDate { get; init; }
        public DateTime? DeliveryDate { get; set; }
    }
}
