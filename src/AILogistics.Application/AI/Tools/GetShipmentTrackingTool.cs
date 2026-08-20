using AILogistics.Application.AI.Abstractions;
using AILogistics.Application.AI.Models;
using AILogistics.Application.DTOs.TrackingEvents;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace AILogistics.Application.AI.Tools
{
    public sealed class GetShipmentTrackingTool : IAiTool
    {
        private readonly ITrackingEventService _trackingEventService;
        private readonly IShipmentService _shipmentService;
        private readonly ICurrentUserService _currentUserService;

        public GetShipmentTrackingTool(ITrackingEventService trackingEventService,
                                        IShipmentService shipmentService,
                                        ICurrentUserService currentUserService)
        {
            _trackingEventService = trackingEventService;
            _shipmentService = shipmentService;
            _currentUserService = currentUserService;
        }

        public string Name => "get_shipment_tracking";

        public string Description => "Gets the tracking history and tracking events for a shipment by its shipment ID";

        public object GetParametersSchema()
        {
            return new
            {
                type = "object",
                properties = new
                {
                    shipmentId = new
                    {
                        type = "integer",
                        description = "The unique ID of shipment."
                    }
                },
                required = new[] { "shipmentId" }
            };
        }

        public async Task<object?> ExecuteAsync(IReadOnlyDictionary<string, object> arguments, CancellationToken cancellationToken = default)
        {
            if (!arguments.TryGetValue("shipmentId", out var shipmentIdValue))
            {
                throw new ArgumentException("shipmentId id required");
            }

            int shipmentId;

            if (shipmentIdValue is JsonElement jsonElement && jsonElement.TryGetInt32(out var parsedId))
            {
                shipmentId = parsedId;
            }
            else if (!int.TryParse(shipmentIdValue?.ToString(),
                out shipmentId))
            {
                throw new ArgumentException("shipmentId must be valid integer");
            }

            var shipment = await _shipmentService.GetShipmentById(shipmentId);

            if (_currentUserService.Role == (int)UserRole.Customer)
            {
                if (!_currentUserService.CustomerId.HasValue)
                {
                    throw new UnauthorizedAccessException("Customer account is not linked to a customer");
                }
                if (shipment.CustomerId != _currentUserService.CustomerId.Value)
                {
                    throw new UnauthorizedAccessException("You are not authorized to access tracking for this shipment.");
                }
            }

            List<TrackingEventResponse> trackingEvents = await _trackingEventService.GetTrackingEventByShipmentId(shipmentId);
            
            List<GetShipmentTrackingToolResult> result = trackingEvents.Select(x => new GetShipmentTrackingToolResult
            {
                Status = x.Status.ToString(),
                Location = x.Location,
                Description = x.Description,
                EventTime = x.EventTime,
            }).ToList();

            return result;

        }
    }
}
