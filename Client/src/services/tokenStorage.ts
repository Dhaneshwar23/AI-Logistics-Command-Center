import type { User } from "@/context/AuthContext";

const ACCESS_TOKEN_KEY = "accessToken";
const REFRESH_TOKEN_KEY = "refreshToken";
const USER_KEY = "authUser";


const getAccessToken = () => {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
}

const setAccessToken = (token: string) => {
    localStorage.setItem(ACCESS_TOKEN_KEY, token)
}

const getRefreshToken = () => {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
}

const setRefreshToken = (token: string) => {
    localStorage.setItem(REFRESH_TOKEN_KEY, token);
}

const getUser = (): User | null => {
        const storedUser = localStorage.getItem(USER_KEY);

        if(!storedUser)
        {
            return null;
        }

        try{
            return JSON.parse(storedUser) as User;
        }
        catch{
            return null;
        }
};

const setUser = (user: User) => {
    localStorage.setItem(USER_KEY, JSON.stringify(user));
}

const clearTokens = () => {
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
}

export default {
    getAccessToken,
    setAccessToken,
    getRefreshToken,
    setRefreshToken,
    getUser,
    setUser,
    clearTokens
}