import { useEffect, useState, type ReactNode } from 'react'
import AuthContext, { type User } from './AuthContext'
import tokenStorage from '@/services/tokenStorage'
import { isTokenExpired } from '@/utils/jwt'
import authService from '@/services/authService'

interface AuthProviderProps {
    children: ReactNode
}

const AuthProvider = ({ children }: AuthProviderProps) => {
    const [user, setUser] = useState<User | null>(() => {
        const storedUser = tokenStorage.getUser();
        const accessToken = tokenStorage.getAccessToken();

        if (!storedUser && !accessToken) {
            return null;
        }

        return storedUser;
    });
    const [isInitializing, setIsInitializing] = useState(true);

    const isAuthenticated = user !== null

    const login = (loggedInUser: User) => {
        tokenStorage.setUser(loggedInUser);
        setUser(loggedInUser);
    }

    const logout = () => {
        tokenStorage.clearTokens()
        setUser(null)
    }

    useEffect(() => {
        const initializeAuth = async () => {

            try {
                const storedUser = tokenStorage.getUser();
                const accessToken = tokenStorage.getAccessToken();
                const refreshToken = tokenStorage.getRefreshToken();


                if (!storedUser || !accessToken) {
                    tokenStorage.clearTokens();
                    setIsInitializing(false);
                    return;
                }

                if (!isTokenExpired(accessToken)) {
                    setUser(storedUser);
                    return;
                }

                if (!refreshToken) {
                    tokenStorage.clearTokens();
                    return;
                }

                const response =
                    await authService.refreshToken(refreshToken);

                tokenStorage.setAccessToken(response.token);
                tokenStorage.setRefreshToken(response.refreshToken);

                const refreshedUser: User = {
                    userId: response.userId,
                    fullName: response.fullName,
                    email: response.email,
                    role: response.role
                };

                tokenStorage.setUser(refreshedUser);
                setUser(refreshedUser);
            }
            catch {
                tokenStorage.clearTokens();
                setUser(null);
            }
            finally {
                setIsInitializing(false);
            }
        };

        initializeAuth();
    }, []);
    return (
        <AuthContext.Provider value={{ user, isAuthenticated, isInitializing, login, logout }}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthProvider