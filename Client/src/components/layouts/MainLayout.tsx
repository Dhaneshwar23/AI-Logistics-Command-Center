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
        <Box>
            <AppHeader onMenuClick={handleMobileDrawerToggle} />

            <Box sx={{ display: 'flex' }}>
                <SideBar mobileOpen={mobileOpen} onClose={handleMobileDrawerClose} drawerWidth={DRAWER_WIDTH} />

                <Box component="main" sx={{ flexGrow: 1, p: { xs: 2, sm: 3 }, minWidth: 0, width: { md: `calc(100% -${DRAWER_WIDTH}px)` }, ml: { md: `${DRAWER_WIDTH}px` }, overflow: 'hidden' }}>
                    <Outlet />
                </Box>
            </Box>
        </Box>
    );
};

export default MainLayout;