import type { Shipment, ShipmentStatus as ShipmentStatusType } from '@/types/shipment';
import { ShipmentStatus } from '@/types/shipment';
import type { CreateTrackingEventRequest } from '@/types/trackingEvent';
import { getShipmentStatusLabel } from '@/utils/shipmentStatus';
import { Box, FormControl, FormHelperText, InputLabel, MenuItem, Select, TextField } from '@mui/material';
import { useState } from 'react';

interface TrackingEventFormProps {
    shipments: Shipment[];
    initialShipmentId: number | null;
    loading: boolean;
    onSubmit: (request: CreateTrackingEventRequest) => Promise<void>;
}

const shipmentStatuses = Object.values(ShipmentStatus);

const TrackingEventForm = ({ shipments, initialShipmentId, loading, onSubmit }: TrackingEventFormProps) => {
    const [shipmentId, setShipmentId] = useState<number | ''>(initialShipmentId ?? '');
    const [status, setStatus] = useState<ShipmentStatusType | ''>('');
    const [location, setLocation] = useState('');
    const [description, setDescription] = useState('');
    const [eventTime, setEventTime] = useState('');
    const [errors, setErrors] = useState<Record<string, string>>({});

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        const nextErrors: Record<string, string> = {};
        const parsedEventTime = new Date(eventTime);

        if (shipmentId === '') nextErrors.shipmentId = 'Shipment is required';
        if (status === '') nextErrors.status = 'Status is required';
        if (!location.trim()) nextErrors.location = 'Location is required';
        if (!description.trim()) nextErrors.description = 'Description is required';
        if (!eventTime || Number.isNaN(parsedEventTime.getTime())) nextErrors.eventTime = 'Valid event date and time is required';

        setErrors(nextErrors);
        if (Object.keys(nextErrors).length > 0 || shipmentId === '' || status === '') return;

        await onSubmit({
            shipmentId,
            status,
            location: location.trim(),
            description: description.trim(),
            eventTime: parsedEventTime.toISOString(),
        });
    };

    return (
        <Box component="form" id="tracking-event-form" onSubmit={handleSubmit} noValidate>
            <FormControl fullWidth margin="normal" error={Boolean(errors.shipmentId)}>
                <InputLabel id="tracking-shipment-label">Shipment</InputLabel>
                <Select labelId="tracking-shipment-label" label="Shipment" value={shipmentId} disabled={loading}
                    onChange={(event) => { setShipmentId(Number(event.target.value)); setErrors((previous) => ({ ...previous, shipmentId: '' })); }}>
                    {shipments.map((shipment) => <MenuItem key={shipment.id} value={shipment.id}>{shipment.shipmentNumber}</MenuItem>)}
                </Select>
                <FormHelperText>{errors.shipmentId}</FormHelperText>
            </FormControl>
            <FormControl fullWidth margin="normal" error={Boolean(errors.status)}>
                <InputLabel id="tracking-status-label">Status</InputLabel>
                <Select labelId="tracking-status-label" label="Status" value={status} disabled={loading}
                    onChange={(event) => { setStatus(Number(event.target.value) as ShipmentStatusType); setErrors((previous) => ({ ...previous, status: '' })); }}>
                    {shipmentStatuses.map((value) => <MenuItem key={value} value={value}>{getShipmentStatusLabel(value)}</MenuItem>)}
                </Select>
                <FormHelperText>{errors.status}</FormHelperText>
            </FormControl>
            <TextField label="Location" value={location} onChange={(event) => { setLocation(event.target.value); setErrors((previous) => ({ ...previous, location: '' })); }}
                error={Boolean(errors.location)} helperText={errors.location} fullWidth margin="normal" required disabled={loading} slotProps={{ htmlInput: { maxLength: 200 } }} />
            <TextField label="Description" value={description} onChange={(event) => { setDescription(event.target.value); setErrors((previous) => ({ ...previous, description: '' })); }}
                error={Boolean(errors.description)} helperText={errors.description} fullWidth margin="normal" required multiline minRows={3} disabled={loading} slotProps={{ htmlInput: { maxLength: 1000 } }} />
            <TextField label="Event Time" type="datetime-local" value={eventTime} onChange={(event) => { setEventTime(event.target.value); setErrors((previous) => ({ ...previous, eventTime: '' })); }}
                error={Boolean(errors.eventTime)} helperText={errors.eventTime} fullWidth margin="normal" required disabled={loading} slotProps={{ inputLabel: { shrink: true } }} />
        </Box>
    );
};

export default TrackingEventForm;
