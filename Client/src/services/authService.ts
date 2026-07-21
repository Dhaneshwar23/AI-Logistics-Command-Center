import api from './api'

export interface LoginRequest{
    email:string
    password:string
}

export interface LoginResponse{
    token:string
    expiresAt:string
    userId: number
    role: number
    fullName:string
    email: string
}

const login = async (credentials:LoginRequest):Promise<LoginResponse> =>{
    const res = await api.post<LoginResponse>('/api/v1/Auth/login', credentials)

    return res.data
}

const authService ={
    login,
}

export default authService