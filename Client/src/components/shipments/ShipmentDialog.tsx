import type { Customer } from "@/types/customer";
import type { CreateShipmentRequest, Shipment, UpdateShipmentRequest } from "@/types/shipment";
import { Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, useMediaQuery, useTheme } from "@mui/material";
import ShipmentForm from "./ShipmentForm";

interface ShipmentDialogProps {
    open: boolean;
    customers: Customer[];
    loading: boolean;
    mode: 'create' | 'edit';
    shipmentToEdit: Shipment | null;
    error: string | null;
    onClose: () => void;
    onSubmit: (request: CreateShipmentRequest | UpdateShipmentRequest) => Promise<void>;
}

const ShipmentDialog = ({ open,
    customers,
    loading,
    mode,
    shipmentToEdit,
    error,
    onClose,
    onSubmit
}: ShipmentDialogProps) => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

    return (
        <Dialog maxWidth="md"
            fullWidth open={open}
            fullScreen={isMobile}
            onClose={loading ? undefined : onClose}>
            <DialogTitle>{mode === 'edit' ? 'Edit Shipment' : 'Add Shipment'}</DialogTitle>
            <DialogContent sx={{ pt: 2.5 }}>
                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                <ShipmentForm
                    open={open}
                    mode={mode}
                    shipmentToEdit={shipmentToEdit}
                    customers={customers}
                    onSubmit={onSubmit}
                    loading={loading}
                />
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={onClose}
                    disabled={loading}
                >
                    Cancel
                </Button>
                <Button
                    type="submit"
                    form="shipment-form"
                    variant="contained"
                    disabled={loading}
                >
                    {loading ? (mode === 'edit' ? "Saving..." : "Creating...") : (mode === 'edit' ? "Save Changes" : "Create")}
                </Button>
            </DialogActions>
        </Dialog>
    )

}

export default ShipmentDialog;
