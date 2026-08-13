import { useState } from 'react'
import { Alert, Box, Button, CircularProgress, Paper, Stack, TextField, Typography } from '@mui/material'
import authService from '@/services/authService';
import tokenStorage from '@/services/tokenStorage';
import useAuth from '@/context/useAuth';
import { useNavigate } from 'react-router-dom';
import getApiErrorMessage from '@/utils/getApiErrorMessage';
import LocalShippingOutlinedIcon from '@mui/icons-material/LocalShippingOutlined';
import RouteOutlinedIcon from '@mui/icons-material/RouteOutlined';
import ShieldOutlinedIcon from '@mui/icons-material/ShieldOutlined';

function LoginPage() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [emailError, setEmailError] = useState("");
    const [passwordError, setPasswordError] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    const navigate = useNavigate();
    const { login } = useAuth();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        let hasError = false;

        setEmailError("");
        setPasswordError("");

        if (!email.trim()) {
            setEmailError("Email is required");
            hasError = true;
        } else if (!emailRegex.test(email.trim())) {
            setEmailError("Enter a valid email address");
            hasError = true;
        }

        if (!password.trim()) {
            setPasswordError("Password is required");
            hasError = true;
        }

        if (hasError) {
            return;
        }

        try {
            setIsSubmitting(true);
            setError(null);

            const res = await authService.login({ email, password });

            tokenStorage.setAccessToken(res.token)
            tokenStorage.setRefreshToken(res.refreshToken)

            const loggedInUser = {
                userId: res.userId,
                email: res.email,
                role: res.role,
                fullName: res.fullName
            };

            login(loggedInUser);

            navigate('/dashboard', { replace: true })
        }
        catch (error: unknown) {
            setError(
                getApiErrorMessage({ error, defaultMessage: "Unable to sign in. Please try again." })
            );
        }
        finally {
            setIsSubmitting(false)
        }
        // const customersResponse = await api.get('/api/v1/customers')

        // console.log(customersResponse.data)
    }

    return (
        <Box sx={{ minHeight: '100vh', display: 'grid', gridTemplateColumns: { xs: '1fr', md: 'minmax(360px, 0.9fr) minmax(480px, 1.1fr)' }, bgcolor: 'background.paper' }}>
            <Box sx={{ display: { xs: 'none', md: 'flex' }, position: 'relative', overflow: 'hidden', flexDirection: 'column', justifyContent: 'space-between', p: { md: 5, lg: 7 }, bgcolor: '#10234A', color: 'white' }}>
                <Box sx={{ position: 'absolute', inset: 0, opacity: 0.16, backgroundImage: 'linear-gradient(#FFFFFF 1px, transparent 1px), linear-gradient(90deg, #FFFFFF 1px, transparent 1px)', backgroundSize: '48px 48px', transform: 'perspective(500px) rotateX(58deg) scale(1.5)', transformOrigin: 'center bottom' }} />
                <Stack direction="row" spacing={1.5} sx={{ alignItems: 'center', position: 'relative' }}>
                    <Box sx={{ width: 42, height: 42, display: 'grid', placeItems: 'center', borderRadius: 1, bgcolor: 'primary.main' }}><LocalShippingOutlinedIcon /></Box>
                    <Box>
                        <Typography variant="h6">AI Logistics</Typography>
                        <Typography variant="caption" sx={{ color: '#B8C8E8' }}>Command Center</Typography>
                    </Box>
                </Stack>
                <Box sx={{ position: 'relative', maxWidth: 540 }}>
                    <Typography component="h1" sx={{ fontSize: { md: '2.6rem', lg: '3.25rem' }, lineHeight: 1.08, fontWeight: 750, letterSpacing: '-0.04em' }}>
                        Keep every shipment moving.
                    </Typography>
                    <Typography sx={{ mt: 2, maxWidth: 470, color: '#C9D5EC', fontSize: '1.05rem', lineHeight: 1.7 }}>
                        A focused operations workspace for customers, shipments, and real-time tracking history.
                    </Typography>
                    <Stack direction="row" spacing={3.5} sx={{ mt: 4, color: '#DCE7FA' }}>
                        <Stack direction="row" spacing={1} sx={{ alignItems: 'center' }}><LocalShippingOutlinedIcon fontSize="small" /><Typography variant="body2">Shipments</Typography></Stack>
                        <Stack direction="row" spacing={1} sx={{ alignItems: 'center' }}><RouteOutlinedIcon fontSize="small" /><Typography variant="body2">Tracking</Typography></Stack>
                        <Stack direction="row" spacing={1} sx={{ alignItems: 'center' }}><ShieldOutlinedIcon fontSize="small" /><Typography variant="body2">Secure access</Typography></Stack>
                    </Stack>
                </Box>
                <Typography variant="caption" sx={{ position: 'relative', color: '#8FA5CD' }}>Logistics operations, clearly coordinated.</Typography>
            </Box>

            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', p: { xs: 2, sm: 4, lg: 6 }, bgcolor: 'background.default' }}>
            <Paper variant="outlined" sx={{ width: '100%', maxWidth: 440, p: { xs: 3, sm: 4.5 }, boxShadow: '0 18px 45px rgba(15, 23, 42, 0.08)' }}>
                <Stack direction="row" spacing={1.25} sx={{ display: { md: 'none' }, alignItems: 'center', mb: 4 }}>
                    <Box sx={{ width: 38, height: 38, display: 'grid', placeItems: 'center', borderRadius: 1, bgcolor: 'primary.main', color: 'white' }}><LocalShippingOutlinedIcon fontSize="small" /></Box>
                    <Box><Typography variant="subtitle1" sx={{ fontWeight: 750, lineHeight: 1.2 }}>AI Logistics</Typography><Typography variant="caption" color="text.secondary">Command Center</Typography></Box>
                </Stack>
                <Typography variant="h4" component="h2">Sign in</Typography>
                <Typography variant="body2" color="text.secondary" sx={{ mt: 1, mb: 3 }}>
                    Enter your credentials to access operations.
                </Typography>
                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                <Box component="form" onSubmit={handleSubmit} noValidate>
                    <TextField
                        label="Email Address"
                        value={email}
                        onChange={(e) => {
                            const value = e.target.value;
                            setEmail(value);
                            setError(null);

                            if (!value.trim()) {
                                setEmailError("");
                            } else if (!emailRegex.test(value.trim())) {
                                setEmailError("Enter a valid email address");
                            } else {
                                setEmailError("");
                            }
                        }}
                        error={Boolean(emailError)}
                        helperText={emailError}
                        type="email"
                        fullWidth
                        margin="normal"
                        autoComplete="email"
                        autoFocus
                    />
                    <TextField label="Password" value={password} onChange={(e) => {
                        setPassword(e.target.value);
                        setPasswordError("");
                        setError(null);
                    }} error={Boolean(passwordError)} helperText={passwordError} fullWidth margin="normal" type="password" autoComplete="current-password" />

                    <Button type="submit" variant="contained" fullWidth size="large" sx={{ mt: 3, minHeight: 48 }} disabled={isSubmitting}>
                        {isSubmitting ? (<CircularProgress size={24} color='inherit' />) : ("Sign In")}
                    </Button>
                </Box>
            </Paper>
            </Box>
        </Box >
    )
}

export default LoginPage
