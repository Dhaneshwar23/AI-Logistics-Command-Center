import { useState, type ReactNode } from 'react'
import AuthContext, { type User } from './AuthContext'
import tokenStorage from '@/services/tokenStorage'

interface AuthProviderProps {
    children: ReactNode
}

const AuthProvider = ({ children } : AuthProviderProps) => {
    const [user, setUser] = useState<User | null>(null)
    
    const isAuthenticated = user !== null

    const login = (loggedInUser: User) => {
        setUser(loggedInUser)
    }

    const logout = () => {
        tokenStorage.clearAccessToken()
        setUser(null)
    }

    return(
        <AuthContext.Provider value={{ user, isAuthenticated, login, logout }}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthProvider