import {Drawer, List, ListItemButton, ListItemIcon, ListItemText} from "@mui/material";
import DashboardIcon from '@mui/icons-material/Dashboard';
import PeopleIcon from '@mui/icons-material/People';
import LocalShippingIcon from '@mui/icons-material/LocalShipping';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import { NavLink } from 'react-router-dom'; 

interface SideBarProps {
    drawerWidth?: number;
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
const SideBar = ({drawerWidth}: SideBarProps) => {
    return (
        <Drawer variant="permanent" sx={{ width: drawerWidth, flexShrink: 0, "& .MuiDrawer-paper":{width: drawerWidth, boxSizing: 'border-box'} }}>
            <List>
                {menuItems.map((item) => (
                    <ListItemButton key={item.path} component={NavLink} to={item.path} sx={{ '&.active': { backgroundColor: "action.selected" } }}>
                        <ListItemIcon>
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