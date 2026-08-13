export interface Shipment {
    id: number;
    shipmentNumber: string;
    customerId: number;
    customerName: string;
    origin: string;
    destination: string;
    weightKg: number;
    shipmentStatus: ShipmentStatus;
    paymentStatus: PaymentStatus;
    pickupDate: string;
    deliveryDate: string;
    createdAt: string;
    updatedAt: string;
    rowVersion: string;
}

export interface ShipmentEditableFields {
    origin: string;
    destination: string;
    weightKg: number;
    pickupDate: string;
    deliveryDate: string;
}

export interface CreateShipmentRequest extends ShipmentEditableFields {
    customerId: number;
}

export interface UpdateShipmentRequest extends ShipmentEditableFields {
    rowVersion: string;
}

export const PaymentStatus = {
    Pending : 0,
    Paid : 1,
    Failed : 2,
} as const;

export type PaymentStatus = (typeof PaymentStatus)[keyof typeof PaymentStatus];

export const ShipmentStatus = {
    Pending: 0,
    PickedUp: 1,
    InTransit: 2,
    OutForDelivery: 3,
    Delivered: 4,
    Cancelled: 5,
    Returned: 6,
} as const;

export type ShipmentStatus = (typeof ShipmentStatus)[keyof typeof ShipmentStatus];
