using AILogistics.Application.DTOs.TrackingEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.Interfaces
{
    public interface ITrackingEventService
    {
        public Task<TrackingEventResponse> CreateTracking(CreateTrackingEvent request);
        public Task<List<TrackingEventResponse>> GetAllTrackingEvents();
        public Task<TrackingEventResponse> GetTrackingEventById(int id);
        public Task<List<TrackingEventResponse>> GetTrackingEventByShipmentId(int shipmentId);
    }
}
