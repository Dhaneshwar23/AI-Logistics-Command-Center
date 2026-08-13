import api from '@/services/api';
import type { PagedResult, PaginationRequest } from '@/types/pagination';
import type { CreateTrackingEventRequest, TrackingEvent } from '@/types/trackingEvent';

const trackingEventService = {
    getAllTrackingEvents: async (request: PaginationRequest): Promise<PagedResult<TrackingEvent>> => {
        const response = await api.get<PagedResult<TrackingEvent>>('/api/v1/TrackingEvents', {
            params: request,
        });
        return response.data;
    },
    getTrackingEventsByShipment: async (shipmentId: number): Promise<TrackingEvent[]> => {
        const response = await api.get<TrackingEvent[]>(`/api/v1/TrackingEvents/shipment/${shipmentId}`);
        return response.data;
    },
    createTrackingEvent: async (request: CreateTrackingEventRequest): Promise<TrackingEvent> => {
        const response = await api.post<TrackingEvent>('/api/v1/TrackingEvents', request);
        return response.data;
    },
};

export default trackingEventService;
