import { Box } from '@mui/material';
import { Outlet } from 'react-router-dom';
import AppHeader from './AppHeader';
import SideBar from './SideBar';
import { useState } from 'react';

export const DRAWER_WIDTH = 240;


const MainLayout = () => {

    const [mobileOpen, setMobileOpen] = useState(false);

    const handleMobileDrawerToggle = () => {
        setMobileOpen((previous) => !previous);
    };

    const handleMobileDrawerClose = () => {
        setMobileOpen(false);
    };

    return (
        <Box sx={{ minHeight: '100vh', bgcolor: 'background.default' }}>
            <AppHeader onMenuClick={handleMobileDrawerToggle} />

            <Box sx={{ display: 'flex' }}>
                <SideBar mobileOpen={mobileOpen} onClose={handleMobileDrawerClose} drawerWidth={DRAWER_WIDTH} />

                <Box component="main" sx={{ flexGrow: 1, minWidth: 0, width: { md: `calc(100% - ${DRAWER_WIDTH}px)` }, ml: { md: `${DRAWER_WIDTH}px` }, overflow: 'hidden' }}>
                    <Box sx={{ width: '100%', maxWidth: 1440, mx: 'auto', p: { xs: 2, sm: 3, lg: 4 } }}>
                        <Outlet />
                    </Box>
                </Box>
            </Box>
        </Box>
    );
};

export default MainLayout;
