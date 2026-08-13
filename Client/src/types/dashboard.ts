export interface DashboardSummary {
    totalShipments: number;
    pendingShipments: number;
    inTransitShipments: number;
    deliveredShipments: number;
    cancelledShipments: number;
    failedPayments: number;
}
