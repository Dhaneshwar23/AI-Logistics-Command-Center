import type { Shipment } from '@/types/shipment';
import { Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, Typography } from '@mui/material';

interface DeleteShipmentDialogProps {
    open: boolean;
    shipment: Shipment | null;
    loading: boolean;
    error: string | null;
    onClose: () => void;
    onConfirm: () => Promise<void>;
}

const DeleteShipmentDialog = ({ open, shipment, loading, error, onClose, onConfirm }: DeleteShipmentDialogProps) => (
    <Dialog open={open} onClose={loading ? undefined : onClose}>
        <DialogTitle>Delete Shipment</DialogTitle>
        <DialogContent>
            {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
            <Typography>
                Are you sure you want to delete <strong>{shipment?.shipmentNumber}</strong>?
            </Typography>
        </DialogContent>
        <DialogActions>
            <Button onClick={onClose} disabled={loading}>Cancel</Button>
            <Button color="error" variant="contained" onClick={onConfirm} disabled={loading || !shipment}>
                {loading ? 'Deleting...' : 'Delete'}
            </Button>
        </DialogActions>
    </Dialog>
);

export default DeleteShipmentDialog;
