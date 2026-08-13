import type { ShipmentStatus } from '@/types/shipment';

export interface TrackingEvent {
    id: number;
    shipmentId: number;
    shipmentNumber: string;
    status: ShipmentStatus;
    location: string;
    description: string;
    eventTime: string;
    createdAt: string;
}

export interface CreateTrackingEventRequest {
    shipmentId: number;
    status: ShipmentStatus;
    location: string;
    description: string;
    eventTime: string;
}
