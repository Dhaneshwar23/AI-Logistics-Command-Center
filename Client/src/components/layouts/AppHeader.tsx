import { AppBar, Toolbar, Typography, Box, Button } from '@mui/material';
import useAuth from '@/context/useAuth';
import { useNavigate } from 'react-router-dom';


const AppHeader = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login', { replace: true });
    }

    return (
        <AppBar position="static">
            <Toolbar>
                <Typography variant="h6" component="h1">
                    AI Logistics
                </Typography>

                <Box sx={{ flexGrow: 1 }} />

                <Typography>
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