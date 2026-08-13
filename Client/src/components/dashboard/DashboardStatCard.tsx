import type { SvgIconComponent } from '@mui/icons-material';
import { Box, Paper, Stack, Typography } from '@mui/material';
import type { Theme } from '@mui/material/styles';

interface DashboardStatCardProps {
    label: string;
    value: number;
    icon: SvgIconComponent;
    color: 'primary' | 'info' | 'success' | 'error';
}

const DashboardStatCard = ({ label, value, icon: Icon, color }: DashboardStatCardProps) => (
    <Paper variant="outlined" sx={{ p: 2.5, height: '100%' }}>
        <Stack direction="row" sx={{ alignItems: 'center', justifyContent: 'space-between' }}>
            <Box>
                <Typography variant="body2" color="text.secondary">{label}</Typography>
                <Typography variant="h4" sx={{ mt: 0.5, fontWeight: 600 }}>{value.toLocaleString()}</Typography>
            </Box>
            <Box sx={(theme: Theme) => ({
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                width: 44,
                height: 44,
                borderRadius: 1,
                color: theme.palette[color].main,
                bgcolor: theme.palette[color].light,
            })}>
                <Icon />
            </Box>
        </Stack>
    </Paper>
);

export default DashboardStatCard;
