import { Navigate, Outlet } from 'react-router-dom';
import useAuth from '@/context/useAuth';
import { Box, CircularProgress } from '@mui/material';

const ProtectedRoute = () => {
    const { isAuthenticated, isInitializing } = useAuth();

    if (isInitializing) {
        return (
            <Box sx={{
                position: "fixed",
                inset: 0,
                display: "flex",
                justifyContent: "center",
                alignItems: "centre",
                //minHeight: "100vh"
            }}>
                <CircularProgress />
            </Box>

        )
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    return <Outlet />;
    // return isAuthenticated
    //     ? <Outlet />
    //     : <Navigate to="/login" replace />;
}

export default ProtectedRoute;