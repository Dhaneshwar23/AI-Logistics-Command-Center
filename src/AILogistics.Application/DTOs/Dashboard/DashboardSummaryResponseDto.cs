using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.DTOs.Dashboard
{
    public class DashboardSummaryResponseDto
    {
        public int TotalShipments { get; set; }
        public int PendingShipments {  get; set; }
        public int InTransitShipments { get; set; }
        public int DeliveredShipments {  get; set; }
        public int CancelledShipments {  get; set; }
        public int FailedPayments {  get; set; }

    }
}
