using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.AI.Models
{
    public sealed class GetShipmentsToolResult
    {
        public IEnumerable<GetShipmentToolResult> Items { get; init; } = Enumerable.Empty<GetShipmentToolResult>();

        public int PageNumber { get; init; }
        public int PageSize {  get; init; }
        public int TotalCount {  get; init; }
        public int Totalpages { get; init; }
    }
}
