import { createTheme } from '@mui/material/styles';

const theme = createTheme({
    palette: {
        primary: {
            main: "#2563EB",
            dark: "#1746B5",
            light: "#E8F0FF",
        },
        secondary: {
            main: "#0F766E",
        },
        background: {
            default: "#F4F7FB",
            paper: "#FFFFFF"
        },
        text: {
            primary: "#172033",
            secondary: "#64748B",
        },
        divider: "#E2E8F0",
    },
    shape: {
        borderRadius: 10,
    },
    typography: {
        fontFamily: 'Inter, "Segoe UI", Roboto, Arial, sans-serif',
        h4: { fontSize: '1.75rem', fontWeight: 700, letterSpacing: '-0.025em' },
        h5: { fontWeight: 700, letterSpacing: '-0.02em' },
        h6: { fontWeight: 650 },
        button: { fontWeight: 650, textTransform: 'none', letterSpacing: 0 },
    },
    components: {
        MuiCssBaseline: {
            styleOverrides: {
                body: { minWidth: 320 },
                '::selection': { backgroundColor: '#BFDBFE' },
            },
        },
        MuiButton: {
            defaultProps: { disableElevation: true },
            styleOverrides: { root: { minHeight: 40, borderRadius: 8, paddingInline: 18 } },
        },
        MuiPaper: {
            styleOverrides: { root: { backgroundImage: 'none' } },
        },
        MuiOutlinedInput: {
            styleOverrides: {
                root: {
                    backgroundColor: '#FFFFFF',
                    '&:hover .MuiOutlinedInput-notchedOutline': { borderColor: '#94A3B8' },
                },
            },
        },
        MuiDialogTitle: {
            styleOverrides: { root: { padding: '22px 24px 14px', fontWeight: 700 } },
        },
        MuiDialogActions: {
            styleOverrides: { root: { padding: '16px 24px 22px', gap: 8 } },
        },
        MuiTableCell: {
            styleOverrides: {
                head: { backgroundColor: '#F8FAFC', color: '#475569', fontWeight: 700, whiteSpace: 'nowrap' },
                root: { borderColor: '#E8EDF4' },
            },
        },
        MuiTableRow: {
            styleOverrides: { root: { '&:last-child td': { borderBottom: 0 } } },
        },
        MuiAlert: {
            styleOverrides: { root: { border: '1px solid', borderColor: 'currentColor', alignItems: 'center' } },
        },
        MuiChip: {
            styleOverrides: { root: { height: 24, borderRadius: 7, fontSize: '0.75rem', fontWeight: 650 } },
        },
        MuiTooltip: {
            defaultProps: { arrow: true },
        },
    },
});

export default theme;
