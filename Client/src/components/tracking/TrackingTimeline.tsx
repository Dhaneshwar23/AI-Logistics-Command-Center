import type { TrackingEvent } from '@/types/trackingEvent';
import { getShipmentStatusColor, getShipmentStatusLabel } from '@/utils/shipmentStatus';
import { Box, Chip, Paper, Stack, Typography } from '@mui/material';

interface TrackingTimelineProps {
    events: TrackingEvent[];
    showShipmentNumber: boolean;
}

const formatEventTime = (value: string): string => {
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? value : date.toLocaleString();
};

const TrackingTimeline = ({ events, showShipmentNumber }: TrackingTimelineProps) => (
    <Stack spacing={0}>
        {events.map((event, index) => (
            <Box key={event.id} sx={{ display: 'flex', gap: { xs: 1.5, sm: 2.5 } }}>
                <Stack sx={{ alignItems: 'center', width: 18, flexShrink: 0 }}>
                    <Box sx={{ width: 14, height: 14, borderRadius: '50%', bgcolor: `${getShipmentStatusColor(event.status)}.main`, mt: 2.5 }} />
                    {index < events.length - 1 && <Box sx={{ width: 2, flexGrow: 1, minHeight: 28, bgcolor: 'divider' }} />}
                </Stack>
                <Paper variant="outlined" sx={{ p: { xs: 2, sm: 2.5 }, mb: 2, flexGrow: 1, minWidth: 0 }}>
                    <Stack direction={{ xs: 'column', sm: 'row' }} spacing={1} sx={{ justifyContent: 'space-between', alignItems: { xs: 'flex-start', sm: 'center' } }}>
                        <Stack direction="row" spacing={1} useFlexGap sx={{ alignItems: 'center', flexWrap: 'wrap' }}>
                            <Chip label={getShipmentStatusLabel(event.status)} color={getShipmentStatusColor(event.status)} size="small" />
                            {showShipmentNumber && <Typography variant="subtitle2">{event.shipmentNumber}</Typography>}
                        </Stack>
                        <Typography variant="body2" color="text.secondary">{formatEventTime(event.eventTime)}</Typography>
                    </Stack>
                    <Typography variant="subtitle1" sx={{ mt: 1 }}>{event.location}</Typography>
                    <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5, overflowWrap: 'anywhere' }}>{event.description}</Typography>
                </Paper>
            </Box>
        ))}
    </Stack>
);

export default TrackingTimeline;
