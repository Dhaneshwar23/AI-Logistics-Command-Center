using AILogistics.Application.AI.Abstractions;
using AILogistics.Application.AI.Models;
using AILogistics.Application.Common;
using AILogistics.Application.DTOs.Shipments;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.AI.Tools
{
    public sealed class GetShipmentsTool : IAiTool
    {
        private readonly IShipmentService _shipmentService;
        private readonly ICurrentUserService _currentUserService;

        public GetShipmentsTool(IShipmentService shipmentService, ICurrentUserService currentUserService)
        {
            _shipmentService = shipmentService;
            _currentUserService = currentUserService;
        }

        public string Name => "get_shipments";
        public string Description => "Gets a paginated list of shipments. Use this when the user asks to list, show, or browse shipments";
        public object GetParametersSchema()
        {
            return new
            {
                type = "object",
                properties = new
                {
                    pageNumber = new
                    {
                        type = "integer",
                        description = "Page number to retrieve. Defaults to 1."
                    },
                    pageSize = new
                    {
                        type = "integer",
                        description = "Number of shipments per page. Defaults to 10 and must not exceed 50."
                    }
                }
            };
        }

        public async Task<object?> ExecuteAsync(IReadOnlyDictionary<string, object> arguments, CancellationToken cancellationToken = default)
        {
            int pageNumber = 1;
            int pageSize = 10;

            if (arguments.TryGetValue("pageNumber", out var pageNumberValue) && int.TryParse(pageNumberValue?.ToString(), out var parsedPageNumber))
            {
                pageNumber = parsedPageNumber;
            }

            if (arguments.TryGetValue("pageSize", out var pageSizeValue) && int.TryParse(pageSizeValue?.ToString(), out var parsedPageSize))
            {
                pageSize = parsedPageSize;
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            pageSize = Math.Clamp(pageSize, 1, 50);

            var pagination = new PaginationRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            //PagedResponse<ShipmentResponse> response = await _shipmentService.GetAllShipments(pagination, cancellationToken);

            PagedResponse<ShipmentResponse> shipmentResponse; ;

            if (_currentUserService.Role == (int)UserRole.Customer)
            {
                if (!_currentUserService.CustomerId.HasValue)
                {
                    throw new UnauthorizedAccessException("Customer account is not linked to a customer");
                }

                shipmentResponse = await _shipmentService.GetShipmentsByCustomerId(pagination, _currentUserService.CustomerId.Value, cancellationToken);
            }

            else
            {
                shipmentResponse = await _shipmentService.GetAllShipments(
                    pagination, cancellationToken);
            }

            GetShipmentsToolResult result = new GetShipmentsToolResult
            {
                Items = shipmentResponse.Items.Select(x => new GetShipmentToolResult
                {
                    Id = x.Id,
                    ShipmentNumber = x.ShipmentNumber,
                    CustomerName = x.CustomerName,
                    Origin = x.Origin,
                    Destination = x.Destination,
                    WeightKg = x.WeightKg,
                    ShipmentStatus = ((ShipmentStatus)x.ShipmentStatus).ToString(),
                    PaymentStatus = ((PaymentStatus)x.PaymentStatus).ToString(),
                    PickupDate = x.PickupDate,
                    DeliveryDate = x.DeliveryDate,
                }),
                PageNumber = shipmentResponse.PageNumber,
                PageSize = shipmentResponse.PageSize,
                TotalCount = shipmentResponse.TotalCount,
                Totalpages = shipmentResponse.TotalPages
            };
            return result;
        }
    }

}
