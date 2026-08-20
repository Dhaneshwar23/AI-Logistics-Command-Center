using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.AI.Models
{
    public sealed class GetShipmentTrackingToolResult
    {
        public string Status { get; init; } = string.Empty;
        public string Location {  get; init; } = string.Empty;
        public string? Description { get; init; }
        public DateTime EventTime {  get; init; }
    }
}
