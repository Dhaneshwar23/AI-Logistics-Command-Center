import { Box } from '@mui/material';
import { Outlet } from 'react-router-dom';
import AppHeader from './AppHeader';
import SideBar from './SideBar';


const MainLayout = () => {

    const drawerWidth = 240;

    return (
        <Box>
            <AppHeader />

            <Box sx={{ display: 'flex' }}>
                <SideBar drawerWidth={drawerWidth} />

                <Box component="main" sx={{ flexGrow: 1, p: 3, minWidth: 0, overflow: 'hidden' }}>
                    <Outlet />
                </Box>
            </Box>
        </Box>
    );
};

export default MainLayout;