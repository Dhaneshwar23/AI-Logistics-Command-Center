using AILogistics.Application.DTOs.TrackingEvents;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.Services
{
    public class TrackingEventService : ITrackingEventService
    {
        private readonly ApplicationDbContext _context;

        public TrackingEventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TrackingEventResponse> CreateTracking(CreateTrackingEvent request)
        {
            Shipment shipment = await _context.Shipment.FirstOrDefaultAsync(x => x.Id == request.ShipmentId);

            if (shipment == null)
            {
                return null;
            }
            else
            {
                TrackingEvent trackingEvent = new TrackingEvent
                {
                    ShipmentId = shipment.Id,
                    Status = request.Status,
                    Location = request.Location,
                    Description = request.Description,
                    EventTime = request.EventTime,
                    Shipment = shipment
                };
                shipment.Status = request.Status;
                await _context.TrackingEvents.AddAsync(trackingEvent);
                await _context.SaveChangesAsync();

                TrackingEventResponse res = MapTrackingEventResponse(trackingEvent);

                return res;
            }
        }

        public async Task<List<TrackingEventResponse>> GetAllTrackingEvents()
        {
            List<TrackingEvent> listOfTrackingEvents = await _context.TrackingEvents.Include(x => x.Shipment).ToListAsync();
            if (!listOfTrackingEvents.Any())
            {
                return [];
            }
            else
            {
                List<TrackingEventResponse> trackingEventResponses = listOfTrackingEvents.Select(trackingEvents => MapTrackingEventResponse(trackingEvents)).ToList();

                return trackingEventResponses;
            }
        }

        public async Task<TrackingEventResponse> GetTrackingEventById(int id)
        {
            TrackingEvent trackingEvent = await _context.TrackingEvents.Include(t => t.Shipment).FirstOrDefaultAsync(x => x.Id == id);
            if (trackingEvent == null)
            {
                return null;
            }
            else
            {
                return MapTrackingEventResponse(trackingEvent);
            }
        }

        public async Task<List<TrackingEventResponse>> GetTrackingEventByShipmentId(int shipmentId)
        {
            List<TrackingEvent> trackingEvents = await _context.TrackingEvents.Include(t => t.Shipment).Where(s => s.ShipmentId == shipmentId).ToListAsync();

            if (!trackingEvents.Any())
            {
                return [];
            }
            else
            {
                List<TrackingEventResponse> response = trackingEvents.Select(trackingEvent => MapTrackingEventResponse(trackingEvent)).ToList();
                return response;
            }
        }

        public TrackingEventResponse MapTrackingEventResponse(TrackingEvent trackingEvent)
        {

            TrackingEventResponse response = new TrackingEventResponse();

            response.Id = trackingEvent.Id;
            response.ShipmentId = trackingEvent.Shipment.Id;
            response.ShipmentNumber = trackingEvent.Shipment.ShipmentNumber;
            response.Status = trackingEvent.Status;
            response.Location = trackingEvent.Location;
            response.Description = trackingEvent.Description;
            response.EventTime = trackingEvent.EventTime;
            response.CreatedAt = trackingEvent.CreatedAt;

            return response;
        }
    }
}
