using AILogistics.Application.AI.Abstractions;
using AILogistics.Application.AI.Models;
using AILogistics.Application.DTOs.Shipments;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AILogistics.Application.AI.Tools
{
    public class GetShipmentTool : IAiTool
    {
        private readonly IShipmentService _shipmentService;
        private readonly ICurrentUserService _currentUserService;

        public GetShipmentTool(IShipmentService shipmentService, ICurrentUserService currentUserService)
        {
            _shipmentService = shipmentService;
            _currentUserService = currentUserService;
        }

        public string Name => "get_shipment";

        public string Description => "Gets a shipment by it's shipment ID.";

        public async Task<object?> ExecuteAsync(IReadOnlyDictionary<string, object> arguments, CancellationToken cancellationToken)
        {
            if (!arguments.TryGetValue("shipmentId", out var shipmentIdValue))
            {
                throw new ArgumentException("shipmentId is required");
            }

            int shipmentId;

            if (shipmentIdValue is JsonElement jsonElement &&
                jsonElement.TryGetInt32(out var parseId))
            {
                shipmentId = parseId;
            }
            else if (!int.TryParse(shipmentIdValue?.ToString(), out shipmentId))
            {
                throw new ArgumentException(" shipmentId must be a valid integer");
            }

            ShipmentResponse shipmentResponse = await _shipmentService.GetShipmentById(shipmentId);

            if (_currentUserService.Role == (int)UserRole.Customer)
            {
                if (!_currentUserService.CustomerId.HasValue)
                {
                    throw new UnauthorizedAccessException("Customer account is not linked to a customer");
                }

                if (shipmentResponse.CustomerId != _currentUserService.CustomerId.Value)
                {

                    throw new UnauthorizedAccessException("You are not authorized to access this shipment");
                }
            }

            GetShipmentToolResult mappedShipment = new GetShipmentToolResult
            {
                Id = shipmentResponse.Id,
                ShipmentNumber = shipmentResponse.ShipmentNumber,
                CustomerName = shipmentResponse.CustomerName,
                Origin = shipmentResponse.Origin,
                Destination = shipmentResponse.Destination,
                WeightKg = shipmentResponse.WeightKg,
                ShipmentStatus = ((ShipmentStatus)shipmentResponse.ShipmentStatus).ToString(),
                PaymentStatus = ((PaymentStatus)shipmentResponse.PaymentStatus).ToString(),
                PickupDate = shipmentResponse.PickupDate,
                DeliveryDate = shipmentResponse.DeliveryDate,
            };

            return mappedShipment;
        }

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
                        description = "The unique ID of the shipment."
                    }
                },
                required = new[] { "shipmentId" }
            };
        }
    }
}
