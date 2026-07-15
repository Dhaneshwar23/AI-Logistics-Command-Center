using AILogistics.Application.Common;
using AILogistics.Application.DTOs.TrackingEvents;

namespace AILogistics.Application.Interfaces;

public interface ITrackingEventService
{
    Task<TrackingEventResponse> CreateTracking(CreateTrackingEvent request);
    Task<PagedResponse<TrackingEventResponse>> GetAllTrackingEvents(PaginationRequest pagination, CancellationToken cancellationToken = default);
    Task<TrackingEventResponse> GetTrackingEventById(int id);
    Task<List<TrackingEventResponse>> GetTrackingEventByShipmentId(int shipmentId);
}
