import authApi from './authApi'

export interface LoginRequest {
    email: string
    password: string
}

export interface LoginResponse {
    token: string
    refreshToken: string
    expiresAt: string
    userId: number
    role: number
    fullName: string
    email: string
}

export interface RefreshTokenRequest {
    refreshToken: string;
}

export interface RefreshTokenResponse {
    token: string
    refreshToken: string
}

const login = async (credentials: LoginRequest): Promise<LoginResponse> => {
    const res = await authApi.post<LoginResponse>('/api/v1/Auth/login', credentials)

    return res.data
}

const refreshToken = async (
    refreshToken: string):
    Promise<LoginResponse> => {
    const response = await authApi.post<LoginResponse>("/api/v1/Auth/refreshToken", {
        refreshToken
    });

    return response.data;
}


const authService = {
    login,
    refreshToken
}

export default authService