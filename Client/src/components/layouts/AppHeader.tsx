import { AppBar, Toolbar, Typography, Box, Button, IconButton } from '@mui/material';
import useAuth from '@/context/useAuth';
import { useNavigate } from 'react-router-dom';
import MenuIcon from '@mui/icons-material/Menu'

interface AppHeaderProps {
    onMenuClick: () => void;
}

const AppHeader = ({ onMenuClick }: AppHeaderProps) => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login', { replace: true });
    }

    return (
        <AppBar position="static">
            <Toolbar>
                <IconButton color="inherit"
                    edge="start"
                    onClick={onMenuClick}
                    sx={{
                        mr: 2,
                        display: { md: "none" }
                    }}>
                    <MenuIcon />
                </IconButton>

                <Typography variant="h6" component="h1">
                    AI Logistics
                </Typography>

                <Box sx={{ flexGrow: 1 }} />

                <Typography
                    sx={{
                        mr: 2,
                        display: { xs: "none", sm: "block" }
                    }}>
                    {user?.fullName}
                </Typography>

                <Button color="inherit" onClick={handleLogout}>
                    Logout
                </Button>
            </Toolbar>
        </AppBar>
    );
};

export default AppHeader;