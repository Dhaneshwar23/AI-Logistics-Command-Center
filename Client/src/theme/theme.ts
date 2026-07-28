import { createTheme } from '@mui/material/styles';

const theme = createTheme({
    palette: {
        primary:{
            main: "#2563EB",
        },
        background: {
            default: "#F6F8FB",
            paper: "#FFFFFF"
        },
    },
    shape: {
        borderRadius: 8,
    },
});

export default theme;