using AILogistics.Application.Common;
using AILogistics.Application.DTOs.Dashboard;
using AILogistics.Application.DTOs.Shipments;
using AILogistics.Application.Exceptions;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AILogistics.Infrastructure.Services;

public class ShipmentService : IShipmentService
{
    private readonly ApplicationDbContext _context;

    public ShipmentService(ApplicationDbContext context) => _context = context;

    public async Task<ShipmentResponse> CreateShipment(CreateShipment request)
    {
        var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Id == request.CustomerId && x.IsActive)
            ?? throw new NotFoundException("No active customer found");

        var shipment = new Shipment
        {
            ShipmentNumber = $"SHP-{Guid.NewGuid():N}",
            CustomerId = request.CustomerId,
            Origin = request.Origin,
            Destination = request.Destination,
            WeightKg = request.WeightKg,
            PaymentStatus = PaymentStatus.Pending,
            Status = ShipmentStatus.Pending,
            PickupDate = request.PickupDate,
            DeliveryDate = request.DeliveryDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Shipment.Add(shipment);
        await _context.SaveChangesAsync();
        return Map(shipment, customer.CompanyName);
    }

    public async Task<bool> DeleteShipment(int id)
    {
        var shipment = await _context.Shipment.SingleOrDefaultAsync(x => x.Id == id);
        if (shipment is null) return false;

        _context.Shipment.Remove(shipment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResponse<ShipmentResponse>> GetAllShipments(
        PaginationRequest pagination,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Shipment.AsNoTracking();
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(shipment => shipment.Id)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(shipment => new ShipmentResponse
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                CustomerId = shipment.CustomerId,
                CustomerName = shipment.Customer.CompanyName,
                Origin = shipment.Origin,
                Destination = shipment.Destination,
                WeightKg = shipment.WeightKg,
                ShipmentStatus = shipment.Status,
                PaymentStatus = shipment.PaymentStatus,
                PickupDate = shipment.PickupDate,
                DeliveryDate = shipment.DeliveryDate,
                CreatedAt = shipment.CreatedAt,
                UpdatedAt = shipment.UpdatedAt,
                RowVersion = shipment.RowVersion
            })
            .ToListAsync(cancellationToken);

        return new PagedResponse<ShipmentResponse>(items, pagination.PageNumber, pagination.PageSize, totalCount);
    }

    public async Task<ShipmentResponse> GetShipmentById(int id)
    {
        var shipment = await _context.Shipment.AsNoTracking()
            .Include(x => x.Customer)
            .SingleOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException("No shipment found");

        return Map(shipment, shipment.Customer.CompanyName);
    }

    public async Task<ShipmentResponse> GetShipmentByShipmentNumber(string shipmentNumber)
    {
        var shipment = await _context.Shipment.AsNoTracking()
            .Include(x => x.Customer)
            .SingleOrDefaultAsync(x => x.ShipmentNumber == shipmentNumber)
            ?? throw new NotFoundException("No shipment found");

        return Map(shipment, shipment.Customer.CompanyName);
    }

    public async Task<ShipmentResponse> UpdateShipment(int id, UpdateShipment request)
    {
        var shipment = await _context.Shipment.Include(x => x.Customer)
            .SingleOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException("No shipment found");

        _context.Entry(shipment).Property(x => x.RowVersion).OriginalValue = request.RowVersion;
        shipment.Origin = request.Origin;
        shipment.Destination = request.Destination;
        shipment.WeightKg = request.WeightKg;
        shipment.PickupDate = request.PickupDate;
        shipment.DeliveryDate = request.DeliveryDate;
        shipment.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConcurrencyException("The shipment was modified by another request. Reload it and retry.", exception);
        }

        return Map(shipment, shipment.Customer.CompanyName);
    }

    public async Task<DashboardSummaryResponseDto> GetDashboardSummaryAsync()
    {
        var totalShipments = await _context.Shipment.CountAsync();

        var pendingShipments = await _context.Shipment
            .CountAsync(x => x.Status == ShipmentStatus.Pending);

        var inTransitShipments = await _context.Shipment
            .CountAsync(x => x.Status == ShipmentStatus.InTransit);

        var deliveredShipments = await _context.Shipment
            .CountAsync(x => x.Status == ShipmentStatus.Delivered);

        var cancelledShipments = await _context.Shipment
            .CountAsync(x => x.Status == ShipmentStatus.Cancelled);

        var failedPayments = await _context.Shipment
            .CountAsync(x => x.PaymentStatus == PaymentStatus.Failed);

        return new DashboardSummaryResponseDto
        {
            TotalShipments = totalShipments,
            PendingShipments = pendingShipments,
            InTransitShipments = inTransitShipments,
            DeliveredShipments = deliveredShipments,
            CancelledShipments = cancelledShipments,
            FailedPayments = failedPayments
        };

    }

    private static ShipmentResponse Map(Shipment shipment, string customerName) => new()
    {
        Id = shipment.Id,
        ShipmentNumber = shipment.ShipmentNumber,
        CustomerId = shipment.CustomerId,
        CustomerName = customerName,
        Origin = shipment.Origin,
        Destination = shipment.Destination,
        PickupDate = shipment.PickupDate,
        DeliveryDate = shipment.DeliveryDate,
        WeightKg = shipment.WeightKg,
        PaymentStatus = shipment.PaymentStatus,
        ShipmentStatus = shipment.Status,
        CreatedAt = shipment.CreatedAt,
        UpdatedAt = shipment.UpdatedAt,
        RowVersion = shipment.RowVersion
    };
}
