import {  useState } from 'react'
import { Box, Button, Paper, TextField, Typography  } from '@mui/material'
import authService from '@/services/authService';
import tokenStorage from '@/services/tokenStorage';

function LoginPage() 
{
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const res = await authService.login({email, password})
        tokenStorage.setAccessToken(res.token)
    //     console.log({userId: res.userId,
    //         role: res.role,
    //         expiresAt: res.expiresAt,
    //         fullName: res.fullName,
    //         email: res.email,
    // })
    }
    return (
        <Box sx={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', p: { xs: 2, sm: 3} }}>
            <Paper elevation = {4} sx={{width:'100%', maxWidth: 420, p:{ xs: 3, sm: 4}, }}>
                <Typography
                variant="h4"
                component="h1"
                sx={{textAlign: 'center', fontWeight:700}}
                >
                AI Logistics
                </Typography>
                <Typography variant="body1" color="text.secondary" sx={{ mt: 1, mb: 3, textAlign: 'center' }}>
                Sign in to your account
                </Typography>
            <Box component="form" onSubmit={handleSubmit}>
                <TextField label="Email Address" value={email} onChange={(e) => setEmail(e.target.value)} type="email" fullWidth margin="normal" />
                <TextField label="Password" value={password} onChange={(e) => setPassword(e.target.value)} fullWidth margin="normal" type="password" />

                <Button type="submit" variant="contained" fullWidth size="large" sx={{ mt: 3 }}>
                        Sign In    
                </Button>
            </Box>
            </Paper>
        </Box>
    )
}

export default LoginPage