import { useState } from 'react'
import { Alert, Box, Button, CircularProgress, Paper, TextField, Typography } from '@mui/material'
import authService from '@/services/authService';
import tokenStorage from '@/services/tokenStorage';
import useAuth from '@/context/useAuth';
import { useNavigate } from 'react-router-dom';
import getApiErrorMessage from '@/utils/getApiErrorMessage';

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
        <Box sx={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', p: { xs: 2, sm: 3 } }}>
            <Paper elevation={4} sx={{ width: '100%', maxWidth: 420, p: { xs: 3, sm: 4 }, }}>
                <Typography
                    variant="h4"
                    component="h1"
                    sx={{ textAlign: 'center', fontWeight: 700 }}
                >
                    AI Logistics
                </Typography>
                <Typography variant="body1" color="text.secondary" sx={{ mt: 1, mb: 3, textAlign: 'center' }}>
                    Sign in to your account
                </Typography>
                <Box component="form" onSubmit={handleSubmit}>
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
                    />
                    <TextField label="Password" value={password} onChange={(e) => setPassword(e.target.value)} error={Boolean(passwordError)} helperText={passwordError} fullWidth margin="normal" type="password" />

                    <Button type="submit" 
                    onChange={(e) => {
                        setPassword(e.target.value);
                        setPasswordError("");
                        setError(null);
                    }}
                    variant="contained" fullWidth size="large" sx={{ mt: 3 }} disabled={isSubmitting}>
                        {isSubmitting ? (<CircularProgress size={24} color='inherit' />) : ("Sign In")}
                    </Button>
                </Box>
                {error && (
                    (<Alert severity="error" sx={{ mb: 2 }}>
                        {error}
                    </Alert>))
                }
            </Paper>
        </Box >
    )
}

export default LoginPage