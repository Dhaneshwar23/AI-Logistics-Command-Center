import DashboardStatCard from '@/components/dashboard/DashboardStatCard';
import dashboardService from '@/services/dashboardService';
import type { DashboardSummary } from '@/types/dashboard';
import getApiErrorMessage from '@/utils/getApiErrorMessage';
import CancelOutlinedIcon from '@mui/icons-material/CancelOutlined';
import CheckCircleOutlinedIcon from '@mui/icons-material/CheckCircleOutlined';
import ErrorOutlinedIcon from '@mui/icons-material/ErrorOutlined';
import Inventory2OutlinedIcon from '@mui/icons-material/Inventory2Outlined';
import LocalShippingOutlinedIcon from '@mui/icons-material/LocalShippingOutlined';
import PendingActionsOutlinedIcon from '@mui/icons-material/PendingActionsOutlined';
import { Alert, Box, CircularProgress, Divider, Grid, Paper, Stack, Typography } from '@mui/material';
import { useEffect, useState } from 'react';

const DashboardPage = () => {
    const [summary, setSummary] = useState<DashboardSummary | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadSummary = async () => {
            try {
                setLoading(true);
                setError(null);
                setSummary(await dashboardService.getSummary());
            } catch (error: unknown) {
                setError(getApiErrorMessage({ error, defaultMessage: 'Unable to load dashboard summary.' }));
            } finally {
                setLoading(false);
            }
        };

        void loadSummary();
    }, []);

    return (
        <Box>
            <Typography variant="h4" sx={{ mb: 3 }}>Dashboard</Typography>

            {loading && <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}><CircularProgress /></Box>}
            {!loading && error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

            {!loading && !error && summary && (
                <>
                    <Grid container spacing={2.5}>
                        <Grid size={{ xs: 12, sm: 6, lg: 3 }}>
                            <DashboardStatCard label="Total Shipments" value={summary.totalShipments} icon={Inventory2OutlinedIcon} color="primary" />
                        </Grid>
                        <Grid size={{ xs: 12, sm: 6, lg: 3 }}>
                            <DashboardStatCard label="In Transit" value={summary.inTransitShipments} icon={LocalShippingOutlinedIcon} color="info" />
                        </Grid>
                        <Grid size={{ xs: 12, sm: 6, lg: 3 }}>
                            <DashboardStatCard label="Delivered" value={summary.deliveredShipments} icon={CheckCircleOutlinedIcon} color="success" />
                        </Grid>
                        <Grid size={{ xs: 12, sm: 6, lg: 3 }}>
                            <DashboardStatCard label="Failed Payments" value={summary.failedPayments} icon={ErrorOutlinedIcon} color="error" />
                        </Grid>
                    </Grid>

                    <Paper variant="outlined" sx={{ mt: 2.5, p: 2.5 }}>
                        <Typography variant="subtitle1" sx={{ fontWeight: 600, mb: 2 }}>Shipment Status</Typography>
                        <Stack direction={{ xs: 'column', sm: 'row' }} spacing={{ xs: 2, sm: 3 }} divider={<Divider orientation="vertical" flexItem sx={{ display: { xs: 'none', sm: 'block' } }} />}>
                            <Stack direction="row" spacing={1.5} sx={{ alignItems: 'center', flex: 1 }}>
                                <PendingActionsOutlinedIcon color="warning" />
                                <Box>
                                    <Typography variant="body2" color="text.secondary">Pending</Typography>
                                    <Typography variant="h6">{summary.pendingShipments.toLocaleString()}</Typography>
                                </Box>
                            </Stack>
                            <Stack direction="row" spacing={1.5} sx={{ alignItems: 'center', flex: 1 }}>
                                <CancelOutlinedIcon color="error" />
                                <Box>
                                    <Typography variant="body2" color="text.secondary">Cancelled</Typography>
                                    <Typography variant="h6">{summary.cancelledShipments.toLocaleString()}</Typography>
                                </Box>
                            </Stack>
                        </Stack>
                    </Paper>
                </>
            )}
        </Box>
    );
};

export default DashboardPage;
