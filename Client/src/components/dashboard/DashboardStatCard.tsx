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
    <Paper variant="outlined" sx={{ p: { xs: 2.25, sm: 2.5 }, height: '100%', borderColor: 'divider', boxShadow: '0 1px 2px rgba(15, 23, 42, 0.03)', transition: 'border-color 150ms ease, box-shadow 150ms ease', '&:hover': { borderColor: 'grey.300', boxShadow: '0 8px 24px rgba(15, 23, 42, 0.06)' } }}>
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
                borderRadius: 1.5,
                color: theme.palette[color].main,
                bgcolor: `${theme.palette[color].main}14`,
            })}>
                <Icon />
            </Box>
        </Stack>
    </Paper>
);

export default DashboardStatCard;
