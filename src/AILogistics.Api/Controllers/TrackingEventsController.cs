using AILogistics.Application.DTOs.TrackingEvents;
using AILogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace AILogistics.Api.Controllers

{
    [ApiController]
    [Route("api/[Controller]")]
    public class TrackingEventsController : ControllerBase
    {
        private readonly ITrackingEventService _trackingEventService;

        public TrackingEventsController(ITrackingEventService trackingEventService)
        {
            _trackingEventService = trackingEventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrackingEvents()
        {
            List<TrackingEventResponse> trackingEventResponses = await _trackingEventService.GetAllTrackingEvents();
            if (!trackingEventResponses.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(trackingEventResponses);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTrackingEventById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            else
            {
                TrackingEventResponse res = await _trackingEventService.GetTrackingEventById(id);
                if (res == null)
                {
                    return NotFound();
                }

                return Ok(res);
            }
        }

        [HttpGet("shipment/{shipmentId}")]
        public async Task<IActionResult> GetTrackingEventsByShipmentId(int shipmentId)
        {
            if (shipmentId <= 0)
            {
                return BadRequest();
            }
            else
            {
                List<TrackingEventResponse> res = await _trackingEventService.GetTrackingEventByShipmentId(shipmentId);
                if(res == null)
                {
                    return NotFound();
                }
                return Ok(res);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateTrackingEvent(CreateTrackingEvent request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            else
            {
                TrackingEventResponse res = await _trackingEventService.CreateTracking(request);
                return Ok(res);
            }
        }
    }
}
