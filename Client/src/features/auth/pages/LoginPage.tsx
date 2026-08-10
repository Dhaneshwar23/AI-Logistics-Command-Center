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
    const [error, setError] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const navigate = useNavigate();
    const { login } = useAuth();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        try {
            e.preventDefault()
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

            navigate('/customers', { replace: true })
        }
        catch (error: unknown) {
            setError(
                getApiErrorMessage({ error, defaultMessage: "Unable to sign in. Please try again." })
            );
        }
        finally{
            setIsSubmitting(false)
        }
        // const customersResponse = await api.get('/api/v1/customers')

        // console.log(customersResponse.data)
    }

    return (
        <Box sx={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', p: { xs: 2, sm: 3 } }}>
            <Paper elevation={4} sx={{ width: '100%', maxWidth: 420, p: { xs: 3, sm: 4 }, }}>
                {error && (
                    (<Alert severity="error" sx={{ mb: 2 }}>
                        {error}
                    </Alert>))
                }
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
                    <TextField label="Email Address" value={email} onChange={(e) => setEmail(e.target.value)} type="email" fullWidth margin="normal" />
                    <TextField label="Password" value={password} onChange={(e) => setPassword(e.target.value)} fullWidth margin="normal" type="password" />

                    <Button type="submit" variant="contained" fullWidth size="large" sx={{ mt: 3 }} disabled={isSubmitting}>
                        {isSubmitting ? (<CircularProgress size={24} color='inherit' />) : ("Sign In")}
                    </Button>
                </Box>
            </Paper>
        </Box >
    )
}

export default LoginPage