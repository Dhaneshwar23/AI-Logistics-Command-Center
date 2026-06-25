using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.DTOs.TrackingEvents
{
  
    public enum ShipmentStatus
    {
        Pending = 0,
        InTransit = 1,
        Delivered = 2,
        Cancelled = 3
    }

    public class CreateTrackingEvent
    {
        public int ShipmentId { get; set; }
        public ShipmentStatus Status { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
    }
}
