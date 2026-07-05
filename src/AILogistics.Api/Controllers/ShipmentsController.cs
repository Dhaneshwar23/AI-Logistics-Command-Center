using AILogistics.Application.DTOs.Shipments;
using AILogistics.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AILogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentsController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetShipments()
        {
            List<ShipmentResponse> shipmentResponses = await _shipmentService.GetAllShipments();

            return Ok(shipmentResponses);
        }

        [HttpGet("{id:int}")]
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

        [HttpPost]
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

        [HttpPut("{shipmentId}")]
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

        [HttpDelete("{shipmentId}")]
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

