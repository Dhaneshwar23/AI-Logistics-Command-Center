using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.DTOs.TrackingEvents
{
    
    public class TrackingEventResponse
    {
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public ShipmentStatus Status { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
