import { Box, Drawer, List, ListItemButton, ListItemIcon, ListItemText, Typography, useMediaQuery, useTheme } from "@mui/material";
import DashboardIcon from '@mui/icons-material/Dashboard';
import PeopleIcon from '@mui/icons-material/People';
import LocalShippingIcon from '@mui/icons-material/LocalShipping';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import { NavLink } from 'react-router-dom';
import LocalShippingOutlinedIcon from '@mui/icons-material/LocalShippingOutlined';

interface SideBarProps {
    drawerWidth?: number;
    mobileOpen: boolean;
    onClose: () => void;
}
const menuItems = [
    {
        label: 'Dashboard',
        path: '/dashboard',
        icon: <DashboardIcon />,

    },
    {
        label: 'Customers',
        path: '/customers',
        icon: <PeopleIcon />,
    },
    {
        label: 'Shipments',
        path: '/shipments',
        icon: <LocalShippingIcon />,
    },
    {
        label: 'Tracking',
        path: '/tracking',
        icon: <LocationOnIcon />,
    }
];
const SideBar = ({ drawerWidth,
    mobileOpen,
    onClose
}: SideBarProps) => {

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("md"));
    const handleNavigate = () => {
        if (isMobile) {
            onClose();
        }
    }
    return (
        <Drawer variant={isMobile ? "temporary" : "persistent"} open={isMobile ? mobileOpen : true} onClose={onClose} ModalProps={{ keepMounted: true, }} sx={{
            "& .MuiDrawer-paper": {
                width: drawerWidth,
                boxSizing: "border-box",
                borderRight: 0,
                bgcolor: '#102A4C',
                color: '#FFFFFF',
            },
        }}>
            <Box sx={{ height: 64, px: 2.25, display: 'flex', alignItems: 'center', gap: 1.25, borderBottom: '1px solid rgba(255,255,255,0.1)' }}>
                <Box sx={{ width: 36, height: 36, borderRadius: 1, display: 'grid', placeItems: 'center', bgcolor: 'primary.main', color: 'white' }}><LocalShippingOutlinedIcon fontSize="small" /></Box>
                <Box>
                    <Typography variant="subtitle2" sx={{ fontWeight: 800, lineHeight: 1.2 }}>AI Logistics</Typography>
                    <Typography variant="caption" sx={{ color: '#AFC2DB' }}>Command Center</Typography>
                </Box>
            </Box>

            <Typography variant="overline" sx={{ px: 2.5, pt: 2.5, pb: 0.75, color: '#829AB7', fontWeight: 700, letterSpacing: '0.08em' }}>Operations</Typography>
            <List sx={{ px: 1.5, py: 0 }}>
                {menuItems.map((item) => (
                    <ListItemButton key={item.path} component={NavLink} to={item.path} onClick={handleNavigate} sx={{ mb: 0.5, minHeight: 44, borderRadius: 1, color: '#C8D6E8', '& .MuiListItemIcon-root': { color: 'inherit' }, '&.active': { backgroundColor: 'primary.main', color: '#FFFFFF', boxShadow: '0 6px 16px rgba(37, 99, 235, 0.28)' }, '&:hover': { bgcolor: 'rgba(255,255,255,0.08)', color: '#FFFFFF' } }}>
                        <ListItemIcon sx={{ minWidth: 40 }}>
                            {item.icon}
                        </ListItemIcon>

                        <ListItemText primary={item.label} />
                    </ListItemButton>

                ))}
            </List>
        </Drawer>

    );
};

export default SideBar;
