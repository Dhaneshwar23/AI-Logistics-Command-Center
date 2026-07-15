using AILogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AILogistics.Application.DTOs.TrackingEvents
{
  
    public class CreateTrackingEvent
    {
        [Range(1, int.MaxValue)]
        public int ShipmentId { get; set; }
        [EnumDataType(typeof(ShipmentStatus))]
        public ShipmentStatus Status { get; set; }
        [Required, StringLength(200)]
        public string Location { get; set; }
        [Required, StringLength(1000)]
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
    }
}
