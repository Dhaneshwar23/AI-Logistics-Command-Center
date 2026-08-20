using AILogistics.Application.AI.Abstractions;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.AI.Tools
{
    public sealed class GetDashboardSummaryTool : IAiTool
    {
        private readonly IShipmentService _shipmentService;
        private readonly ICurrentUserService _currentUserService;

        public GetDashboardSummaryTool(IShipmentService shipmentService, ICurrentUserService currentUserService)
        {
            _shipmentService = shipmentService;
            _currentUserService = currentUserService;
        }

        public string Name => "get_dashboard_summary";
        public string Description => "gets the current logistics dashboard summary including shipment counts and failed payments.";

        public object GetParametersSchema()
        {
            return new
            {
                type = "object",
                properties = new
                {
                }
            };
        }

        public async Task<Object?> ExecuteAsync(IReadOnlyDictionary<string, object> arguments, CancellationToken cancellationToken = default)
        {
            if (_currentUserService.Role == (int)UserRole.Customer)
            {
                throw new UnauthorizedAccessException("Customer are not authorized to access the global dashboard summary");
            }
            var summary = await _shipmentService.GetDashboardSummaryAsync();

            return summary;
        }

    }
}
