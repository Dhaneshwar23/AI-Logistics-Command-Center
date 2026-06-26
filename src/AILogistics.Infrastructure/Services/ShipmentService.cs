using AILogistics.Application.DTOs.Shipments;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.Services
{


    public class ShipmentService : IShipmentService
    {

        private readonly ApplicationDbContext _context;

        public ShipmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShipmentResponse> CreateShipment(CreateShipment request)
        {
            Customer customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.CustomerId);
            if (customer == null)
            {
                return null;
            }
            else
            {
                var shipmentNumber = $"SHP-{DateTime.Now.Ticks}";
                Shipment shipment = new Shipment();

                shipment.ShipmentNumber = shipmentNumber;
                shipment.CustomerId = request.CustomerId;
                shipment.Origin = request.Origin;
                shipment.Destination = request.Destination;
                shipment.WeightKg = request.WeightKg;
                shipment.PaymentStatus = PaymentStatus.Pending;
                shipment.Status = ShipmentStatus.Pending;
                shipment.PickupDate = request.PickupDate;
                shipment.DeliveryDate = request.DeliveryDate;
                shipment.CreatedAt = DateTime.UtcNow;
                shipment.UpdatedAt = DateTime.UtcNow;



                await _context.Shipment.AddAsync(shipment);
                await _context.SaveChangesAsync();

                ShipmentResponse response = MapShipmentResponse(shipment, customer);
                return response;
            }
        }


        public async Task<bool> DeleteShipment(int id)
        {
            Shipment shipment = await _context.Shipment.FirstOrDefaultAsync(x => x.Id == id);

            if (shipment == null)
            {
                return false;
            }

            _context.Shipment.Remove(shipment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ShipmentResponse>> GetAllShipments()
        {
            List<Shipment> shipments = await _context.Shipment
                                            .Include(x => x.Customer)
                                            .ToListAsync();
            var response = shipments.Select(shipment => MapShipmentResponse(shipment, shipment.Customer))
                .ToList();

            return response;
        }

        public async Task<ShipmentResponse> GetShipmentById(int id)
        {
            Shipment shipment = await _context.Shipment
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (shipment == null)
            {
                throw new Exception("Database Exploded");
            }
            return MapShipmentResponse(shipment, shipment.Customer);

        }

        public async Task<ShipmentResponse> GetShipmentByShipmentNumber(string shipmentNumber)
        {
            Shipment shipment = await _context.Shipment
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.ShipmentNumber == shipmentNumber);

            if (shipment == null)
            {
                return null;
            }

            return MapShipmentResponse(shipment, shipment.Customer);
        }

        public async Task<ShipmentResponse> UpdateShipment(int id, UpdateShipment request)
        {
            Shipment shipment = await _context.Shipment
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (shipment == null)
            {
                return null;
            }


            shipment.Origin = request.Origin;
            shipment.Destination = request.Destination;
            shipment.WeightKg = request.WeightKg;
            shipment.PickupDate = request.PickupDate;
            shipment.DeliveryDate = request.DeliveryDate;

            await _context.SaveChangesAsync();

            return MapShipmentResponse(shipment, shipment.Customer);


        }

        private ShipmentResponse MapShipmentResponse(Shipment shipment, Customer customer)
        {
            ShipmentResponse shipmentResponse = new ShipmentResponse
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                CustomerId = shipment.CustomerId,
                Origin = shipment.Origin,
                Destination = shipment.Destination,
                CustomerName = customer.CompanyName,
                PickupDate = shipment.PickupDate,
                DeliveryDate = shipment.DeliveryDate,
                WeightKg = shipment.WeightKg,
                PaymentStatus = shipment.PaymentStatus,
                ShipmentStatus = shipment.Status,
                CreatedAt = shipment.CreatedAt,
                UpdatedAt = shipment.UpdatedAt

            };

            return shipmentResponse;
        }
    }
}
