using AILogistics.Api.Extensions;
using AILogistics.Api.Filters;
using AILogistics.Application.DTOs.Shipments;
using AILogistics.Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using AILogistics.Application.Common;

namespace AILogistics.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Policy = AuthenticationExtensions.OperationsPolicy)]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentsController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpGet]
        [OutputCache(PolicyName = OutputCachingExtensions.GeneralPolicy,
            Tags = new[] { OutputCachingExtensions.ShipmentTag })]
        public async Task<IActionResult> GetShipments([FromQuery] PaginationRequest pagination, CancellationToken cancellationToken)
        {
            var shipmentResponses = await _shipmentService.GetAllShipments(pagination, cancellationToken);

            return Ok(shipmentResponses);
        }

        [HttpGet("{id:int}")]
        [OutputCache(PolicyName = OutputCachingExtensions.GeneralPolicy,
            Tags = new[] { OutputCachingExtensions.ShipmentTag })]
        public async Task<IActionResult> GetShipmentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            else
            {
                ShipmentResponse shipmentResponse = await _shipmentService.GetShipmentById(id);
                if (shipmentResponse == null)
                {
                    return NotFound();
                }

                return Ok(shipmentResponse);

            }

        }

        [HttpGet("by-number/{shipmentNumber}")]
        [OutputCache(PolicyName = OutputCachingExtensions.GeneralPolicy,
            Tags = new[] { OutputCachingExtensions.ShipmentTag })]
        public async Task<IActionResult> GetShipmentByShipmentNumber(string shipmentNumber)
        {
            if (string.IsNullOrEmpty(shipmentNumber))
            {
                return BadRequest();
            }
            else
            {
                ShipmentResponse shipmentResponse = await _shipmentService.GetShipmentByShipmentNumber(shipmentNumber);
                if (shipmentResponse == null)
                {
                    return NotFound();
                }
                return Ok(shipmentResponse);
            }
        }
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [InvalidateOutputCache(OutputCachingExtensions.ShipmentTag)]
        public async Task<IActionResult> CreateShipment(CreateShipment request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            else
            {
                ShipmentResponse shipmentResponse = await _shipmentService.CreateShipment(request);

                return Ok(shipmentResponse);
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("{shipmentId}")]
        [InvalidateOutputCache(OutputCachingExtensions.ShipmentTag)]
        public async Task<IActionResult> UpdateShipment(int shipmentId, UpdateShipment request)
        {
            if (request == null || shipmentId <= 0)
            {
                return BadRequest();
            }
            else
            {
                ShipmentResponse response = await _shipmentService.UpdateShipment(shipmentId, request);

                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete("{shipmentId}")]
        [InvalidateOutputCache(OutputCachingExtensions.ShipmentTag)]
        public async Task<IActionResult> DeleteShipment(int shipmentId)
        {
            if (shipmentId <= 0)
            {
                return BadRequest();
            }
            else
            {
                var res = await _shipmentService.DeleteShipment(shipmentId);

                if(res == true)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}

