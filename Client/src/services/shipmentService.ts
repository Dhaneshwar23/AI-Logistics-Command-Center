import api from '@/services/api'
import type { PagedResult, PaginationRequest } from '@/types/pagination'
import type { CreateShipmentRequest, Shipment, UpdateShipmentRequest } from '@/types/shipment'

const shipmentService = {
    getAllShipments: async (request: PaginationRequest)
        : Promise<PagedResult<Shipment>> => {
        const res = await api.get<PagedResult<Shipment>>(
            "/api/v1/shipments", {
            params: request,
        }

        );
        return res.data;
    },
    createShipment: async (request: CreateShipmentRequest): Promise<Shipment> => {
        const res = await api.post<Shipment>("/api/v1/shipments", 
            request,
        );
        return res.data;
    },
    updateShipment: async (id: number, request: UpdateShipmentRequest): Promise<Shipment> => {
        const res = await api.put<Shipment>(`/api/v1/shipments/${id}`, request);
        return res.data;
    },
    deleteShipment: async (id: number): Promise<void> => {
        await api.delete(`/api/v1/shipments/${id}`);
    }
};

export default shipmentService;
