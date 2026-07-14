using AILogistics.Api.Extensions;
using AILogistics.Api.Filters;
using AILogistics.Application.DTOs.TrackingEvents;
using AILogistics.Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
namespace AILogistics.Api.Controllers

{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TrackingEventsController : ControllerBase
    {
        private readonly ITrackingEventService _trackingEventService;

        public TrackingEventsController(ITrackingEventService trackingEventService)
        {
            _trackingEventService = trackingEventService;
        }

        [HttpGet]
        [OutputCache(PolicyName = OutputCachingExtensions.GeneralPolicy,
           Tags = new[] { OutputCachingExtensions.TrackingEventsTag } )]
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
        [OutputCache(PolicyName = OutputCachingExtensions.GeneralPolicy,
           Tags = new[] { OutputCachingExtensions.TrackingEventsTag })]
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
        [OutputCache(PolicyName = OutputCachingExtensions.GeneralPolicy,
           Tags = new[] { OutputCachingExtensions.TrackingEventsTag })]
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
        [InvalidateOutputCache(OutputCachingExtensions.TrackingEventsTag)]
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
