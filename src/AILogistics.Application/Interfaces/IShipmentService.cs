using AILogistics.Application.DTOs.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.Interfaces
{
    public interface IShipmentService
    {
        public Task<ShipmentResponse> CreateShipment(CreateShipment request);
        public Task<List<ShipmentResponse>> GetAllShipments();
        public Task<ShipmentResponse> GetShipmentById (int id);
        public Task<ShipmentResponse> GetShipmentByShipmentNumber(string shipmentNumber);
        public Task<ShipmentResponse> UpdateShipment(int shipmentId, UpdateShipment request);
        public Task<bool> DeleteShipment(int shipmentId);

    }
}
