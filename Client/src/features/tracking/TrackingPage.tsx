import TrackingEventDialog from '@/components/tracking/TrackingEventDialog';
import TrackingTimeline from '@/components/tracking/TrackingTimeline';
import shipmentService from '@/services/shipmentService';
import trackingEventService from '@/services/trackingEventService';
import type { PagedResult } from '@/types/pagination';
import type { Shipment } from '@/types/shipment';
import type { CreateTrackingEventRequest, TrackingEvent } from '@/types/trackingEvent';
import getApiErrorMessage from '@/utils/getApiErrorMessage';
import AddIcon from '@mui/icons-material/Add';
import { Alert, Box, Button, CircularProgress, FormControl, InputLabel, MenuItem, Select, Stack, TablePagination, Typography } from '@mui/material';
import { useCallback, useEffect, useState } from 'react';

const TrackingPage = () => {
    const [shipments, setShipments] = useState<Shipment[]>([]);
    const [selectedShipmentId, setSelectedShipmentId] = useState<number | ''>('');
    const [pagedEvents, setPagedEvents] = useState<PagedResult<TrackingEvent> | null>(null);
    const [shipmentEvents, setShipmentEvents] = useState<TrackingEvent[]>([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [dialogOpen, setDialogOpen] = useState(false);
    const [createLoading, setCreateLoading] = useState(false);
    const [createError, setCreateError] = useState<string | null>(null);

    const events = selectedShipmentId === '' ? (pagedEvents?.items ?? []) : shipmentEvents;

    const loadEvents = useCallback(async () => {
        try {
            setLoading(true);
            setError(null);
            if (selectedShipmentId === '') {
                setPagedEvents(await trackingEventService.getAllTrackingEvents({ pageNumber, pageSize }));
            } else {
                setShipmentEvents(await trackingEventService.getTrackingEventsByShipment(selectedShipmentId));
            }
        } catch (error: unknown) {
            setError(getApiErrorMessage({ error, defaultMessage: 'Unable to load tracking events.' }));
        } finally {
            setLoading(false);
        }
    }, [selectedShipmentId, pageNumber, pageSize]);

    useEffect(() => {
        const loadShipments = async () => {
            try {
                const response = await shipmentService.getAllShipments({ pageNumber: 1, pageSize: 100 });
                setShipments(response.items);
            } catch (error: unknown) {
                setError(getApiErrorMessage({ error, defaultMessage: 'Unable to load shipments.' }));
            }
        };
        loadShipments();
    }, []);

    useEffect(() => {
        void Promise.resolve().then(loadEvents);
    }, [loadEvents]);

    const handleCreate = async (request: CreateTrackingEventRequest) => {
        try {
            setCreateLoading(true);
            setCreateError(null);
            await trackingEventService.createTrackingEvent(request);
            setDialogOpen(false);
            if (selectedShipmentId === '') {
                setPageNumber(1);
                setPagedEvents(await trackingEventService.getAllTrackingEvents({ pageNumber: 1, pageSize }));
            } else {
                setShipmentEvents(await trackingEventService.getTrackingEventsByShipment(selectedShipmentId));
            }
        } catch (error: unknown) {
            setCreateError(getApiErrorMessage({ error, defaultMessage: 'Unable to create tracking event.' }));
        } finally {
            setCreateLoading(false);
        }
    };

    return (
        <Box>
            <Stack direction={{ xs: 'column', sm: 'row' }} sx={{ mb: 3, justifyContent: 'space-between', alignItems: { xs: 'stretch', sm: 'center' } }}>
                <Typography variant="h4" sx={{ mb: { xs: 2, sm: 0 } }}>Tracking</Typography>
                <Button variant="contained" startIcon={<AddIcon />} onClick={() => { setCreateError(null); setDialogOpen(true); }} sx={{ width: { xs: '100%', sm: 'auto' } }}>
                    Add Tracking Event
                </Button>
            </Stack>

            <FormControl fullWidth sx={{ mb: 3, maxWidth: { sm: 480 } }}>
                <InputLabel id="shipment-filter-label">Shipment</InputLabel>
                <Select labelId="shipment-filter-label" label="Shipment" value={selectedShipmentId}
                    onChange={(event) => { setSelectedShipmentId(String(event.target.value) === '' ? '' : Number(event.target.value)); setPageNumber(1); }}>
                    <MenuItem value=""><em>All shipments</em></MenuItem>
                    {shipments.map((shipment) => <MenuItem key={shipment.id} value={shipment.id}>{shipment.shipmentNumber}</MenuItem>)}
                </Select>
            </FormControl>

            {loading && <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}><CircularProgress /></Box>}
            {!loading && error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
            {!loading && !error && events.length === 0 && (
                <Alert severity="info">{selectedShipmentId === '' ? 'No tracking events found.' : 'No tracking history exists for this shipment yet.'}</Alert>
            )}
            {!loading && !error && events.length > 0 && (
                <>
                    <TrackingTimeline events={events} showShipmentNumber={selectedShipmentId === ''} />
                    {selectedShipmentId === '' && <TablePagination component="div" count={pagedEvents?.totalCount ?? 0} page={pageNumber - 1} rowsPerPage={pageSize}
                        onPageChange={(_event, newPage) => setPageNumber(newPage + 1)}
                        onRowsPerPageChange={(event) => { setPageSize(Number(event.target.value)); setPageNumber(1); }} rowsPerPageOptions={[5, 10, 25]} />}
                </>
            )}

            <TrackingEventDialog open={dialogOpen} shipments={shipments} initialShipmentId={selectedShipmentId === '' ? null : selectedShipmentId}
                loading={createLoading} error={createError} onClose={() => { setDialogOpen(false); setCreateError(null); }} onSubmit={handleCreate} />
        </Box>
    );
};

export default TrackingPage;
