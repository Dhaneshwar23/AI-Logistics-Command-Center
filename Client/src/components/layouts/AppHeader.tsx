import { AppBar, Toolbar, Typography, Box } from '@mui/material';
import useAuth from '@/context/useAuth';


const AppHeader = () => {
    const { user } = useAuth();
    return (
        <AppBar position="static">
            <Toolbar>
                <Typography variant="h6" component="h1">
                    AI Logistics
                </Typography>

                <Box sx={{flexGrow: 1}} />

                <Typography>
                    {user?.fullName || 'Dhaneshwar'}
                </Typography>
            </Toolbar>
        </AppBar>
    );
};

export default AppHeader;