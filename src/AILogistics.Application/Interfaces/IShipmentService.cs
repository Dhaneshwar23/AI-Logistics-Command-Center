using AILogistics.Application.Common;
using AILogistics.Application.DTOs.Dashboard;
using AILogistics.Application.DTOs.Shipments;

namespace AILogistics.Application.Interfaces;

public interface IShipmentService
{
    Task<ShipmentResponse> CreateShipment(CreateShipment request);
    Task<PagedResponse<ShipmentResponse>> GetAllShipments(PaginationRequest pagination, CancellationToken cancellationToken = default);
    Task<ShipmentResponse> GetShipmentById(int id);
    Task<ShipmentResponse> GetShipmentByShipmentNumber(string shipmentNumber);
    Task<ShipmentResponse> UpdateShipment(int shipmentId, UpdateShipment request);
    Task<bool> DeleteShipment(int shipmentId);
    Task<DashboardSummaryResponseDto> GetDashboardSummaryAsync();
}
