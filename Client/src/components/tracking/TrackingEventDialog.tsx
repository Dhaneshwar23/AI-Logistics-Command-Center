import type { Shipment } from '@/types/shipment';
import type { CreateTrackingEventRequest } from '@/types/trackingEvent';
import { Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, useMediaQuery, useTheme } from '@mui/material';
import TrackingEventForm from './TrackingEventForm';

interface TrackingEventDialogProps {
    open: boolean;
    shipments: Shipment[];
    initialShipmentId: number | null;
    loading: boolean;
    error: string | null;
    onClose: () => void;
    onSubmit: (request: CreateTrackingEventRequest) => Promise<void>;
}

const TrackingEventDialog = ({ open, shipments, initialShipmentId, loading, error, onClose, onSubmit }: TrackingEventDialogProps) => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

    return (
        <Dialog open={open} onClose={loading ? undefined : onClose} maxWidth="sm" fullWidth fullScreen={isMobile}>
            <DialogTitle>Add Tracking Event</DialogTitle>
            <DialogContent sx={{ pt: 2.5 }}>
                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                {open && <TrackingEventForm shipments={shipments} initialShipmentId={initialShipmentId} loading={loading} onSubmit={onSubmit} />}
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} disabled={loading}>Cancel</Button>
                <Button type="submit" form="tracking-event-form" variant="contained" disabled={loading}>
                    {loading ? 'Adding...' : 'Add Event'}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default TrackingEventDialog;
