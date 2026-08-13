import { AppBar, Toolbar, Typography, Box, Button, IconButton, Avatar, Stack } from '@mui/material';
import useAuth from '@/context/useAuth';
import { useNavigate } from 'react-router-dom';
import MenuIcon from '@mui/icons-material/Menu'
import LogoutOutlinedIcon from '@mui/icons-material/LogoutOutlined';

interface AppHeaderProps {
    onMenuClick: () => void;
}

const AppHeader = ({ onMenuClick }: AppHeaderProps) => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();
    const roleLabels: Record<number, string> = {
        0: 'Admin',
        1: 'Manager',
        2: 'Customer',
    };
    const roleLabel = user ? roleLabels[user.role] : undefined;

    const handleLogout = () => {
        logout();
        navigate('/login', { replace: true });
    }

    return (
        <AppBar position="sticky" elevation={0} sx={{ width: { md: `calc(100% - 240px)` }, ml: { md: '240px' }, bgcolor: 'rgba(255,255,255,0.96)', color: 'text.primary', borderBottom: 1, borderColor: 'divider', zIndex: (theme) => theme.zIndex.drawer - 1, backdropFilter: 'blur(8px)' }}>
            <Toolbar sx={{ minHeight: { xs: 60, sm: 64 } }}>
                <IconButton color="inherit"
                    edge="start"
                    onClick={onMenuClick}
                    sx={{
                        mr: 2,
                        display: { md: "none" }
                    }}>
                    <MenuIcon />
                </IconButton>

                <Typography variant="subtitle1" component="div" sx={{ display: { md: 'none' }, fontWeight: 700 }}>AI Logistics</Typography>

                <Box sx={{ flexGrow: 1 }} />

                <Stack direction="row" spacing={1.25} sx={{ alignItems: 'center', mr: { xs: 0.5, sm: 2 } }}>
                    <Avatar sx={{ width: 34, height: 34, bgcolor: 'primary.light', color: 'primary.dark', fontSize: 14, fontWeight: 700 }}>
                        {user?.fullName?.charAt(0).toUpperCase() ?? 'U'}
                    </Avatar>
                    <Box sx={{ display: { xs: 'none', sm: 'block' }, lineHeight: 1.2 }}>
                        <Typography variant="body2" sx={{ fontWeight: 650 }}>{user?.fullName}</Typography>
                        {roleLabel && <Typography variant="caption" color="text.secondary">{roleLabel}</Typography>}
                    </Box>
                </Stack>

                <Button color="inherit" onClick={handleLogout} startIcon={<LogoutOutlinedIcon />} sx={{ color: 'text.secondary', minWidth: { xs: 40, sm: 'auto' }, px: { xs: 1, sm: 2 } }}>
                    <Box component="span" sx={{ display: { xs: 'none', sm: 'inline' } }}>Logout</Box>
                </Button>
            </Toolbar>
        </AppBar>
    );
};

export default AppHeader;
