using AILogistics.Application.Common;
using AILogistics.Application.DTOs.TrackingEvents;
using AILogistics.Application.Exceptions;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AILogistics.Infrastructure.Services;

public class TrackingEventService : ITrackingEventService
{
    private readonly ApplicationDbContext _context;

    public TrackingEventService(ApplicationDbContext context) => _context = context;

    public async Task<TrackingEventResponse> CreateTracking(CreateTrackingEvent request)
    {
        var shipment = await _context.Shipment.SingleOrDefaultAsync(x => x.Id == request.ShipmentId)
            ?? throw new NotFoundException("No shipment found");

        var trackingEvent = new TrackingEvent
        {
            ShipmentId = shipment.Id,
            Status = request.Status,
            Location = request.Location,
            Description = request.Description,
            EventTime = request.EventTime,
            Shipment = shipment,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        shipment.Status = request.Status;
        shipment.UpdatedAt = DateTime.UtcNow;
        _context.TrackingEvents.Add(trackingEvent);
        await _context.SaveChangesAsync();
        return Map(trackingEvent);
    }

    public async Task<PagedResponse<TrackingEventResponse>> GetAllTrackingEvents(
        PaginationRequest pagination,
        CancellationToken cancellationToken = default)
    {
        var query = _context.TrackingEvents.AsNoTracking();
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(item => item.EventTime)
            .ThenByDescending(item => item.Id)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(item => new TrackingEventResponse
            {
                Id = item.Id,
                ShipmentId = item.ShipmentId,
                ShipmentNumber = item.Shipment.ShipmentNumber,
                Status = item.Status,
                Location = item.Location,
                Description = item.Description,
                EventTime = item.EventTime,
                CreatedAt = item.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new PagedResponse<TrackingEventResponse>(items, pagination.PageNumber, pagination.PageSize, totalCount);
    }

    public async Task<TrackingEventResponse> GetTrackingEventById(int id)
    {
        var item = await _context.TrackingEvents.AsNoTracking()
            .Include(x => x.Shipment)
            .SingleOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException("No tracking event found");
        return Map(item);
    }

    public async Task<List<TrackingEventResponse>> GetTrackingEventByShipmentId(int shipmentId)
    {
        return await _context.TrackingEvents.AsNoTracking()
            .Where(x => x.ShipmentId == shipmentId)
            .OrderByDescending(x => x.EventTime)
            .Select(item => new TrackingEventResponse
            {
                Id = item.Id,
                ShipmentId = item.ShipmentId,
                ShipmentNumber = item.Shipment.ShipmentNumber,
                Status = item.Status,
                Location = item.Location,
                Description = item.Description,
                EventTime = item.EventTime,
                CreatedAt = item.CreatedAt
            })
            .ToListAsync();
    }

    private static TrackingEventResponse Map(TrackingEvent item) => new()
    {
        Id = item.Id,
        ShipmentId = item.ShipmentId,
        ShipmentNumber = item.Shipment.ShipmentNumber,
        Status = item.Status,
        Location = item.Location,
        Description = item.Description,
        EventTime = item.EventTime,
        CreatedAt = item.CreatedAt
    };
}
