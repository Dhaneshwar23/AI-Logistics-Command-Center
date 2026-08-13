import {
    ShipmentStatus,
    PaymentStatus
} from "@/types/shipment"

export const getShipmentStatusLabel = (status: ShipmentStatus
): string => {
    switch (status) {
        case ShipmentStatus.Pending:
            return "Pending";
        case ShipmentStatus.PickedUp:
            return "Picked Up";
        case ShipmentStatus.InTransit:
            return "In Transit";
        case ShipmentStatus.OutForDelivery:
            return "Out for Delivery";
        case ShipmentStatus.Delivered:
            return "Delivered"
        case ShipmentStatus.Cancelled:
            return "Cancelled";
        case ShipmentStatus.Returned:
            return "Returned";

        default:
            return "Unknown";
    }
};

export const getPaymentStatusLabel = (status: PaymentStatus): string => {
    switch (status) {
        case PaymentStatus.Pending:
            return "Pending";
        case PaymentStatus.Paid:
            return "Paid";
        case PaymentStatus.Failed:
            return "Failed";
        default:
            return "unknown";
    }
};

export const getShipmentStatusColor = (status: ShipmentStatus) => {
    switch (status) {
        case ShipmentStatus.Pending:
            return "warning";
        case ShipmentStatus.PickedUp:
            return "info";
        case ShipmentStatus.InTransit:
            return "primary";
        case ShipmentStatus.OutForDelivery:
            return "secondary";
        case ShipmentStatus.Delivered:
            return "success";
        case ShipmentStatus.Cancelled:
            return "error";
        case ShipmentStatus.Returned:
            return "error";
        default:
            return "default";
    }
};

export const getPaymentStatusColor = (status: PaymentStatus) => {
    switch (status) {
        case PaymentStatus.Pending:
            return "warning";
        case PaymentStatus.Paid:
            return "success";
        case PaymentStatus.Failed:
            return "error";

        default:
            return "default";
    }
};