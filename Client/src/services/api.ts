import axios from 'axios'
import tokenStorage from './tokenStorage'
import type { InternalAxiosRequestConfig } from 'axios'
import authService from './authService';

interface RetryRequestConfig extends InternalAxiosRequestConfig {
    _retry?: boolean;
}

const api = axios.create({
    //baseURL: 'https://localhost:7009',
    baseURL: 'https://ai-logistics-api-dk-dqbxfmezfaafc2gn.southeastasia-01.azurewebsites.net',
    headers: {
        'Content-Type': 'application/json',
    },
})

api.interceptors.request.use((config) => {
    const token = tokenStorage.getAccessToken()

    if (token) {
        config.headers.Authorization = `Bearer ${token}`
    }

    return config
});

api.interceptors.response.use((response) => response,
    async (error) => {
        const originalRequest =
            error.config as RetryRequestConfig;

        if (
            error.response?.status === 401 &&
            !originalRequest._retry
        ) {
            originalRequest._retry = true;

            const refreshToken =
                tokenStorage.getRefreshToken();

            if (!refreshToken) {
                tokenStorage.clearTokens();
                window.location.href = "/login";
                return Promise.reject(error);
            }

            try {
                const refreshResponse =
                    await authService.refreshToken(refreshToken);

                tokenStorage.setAccessToken(refreshResponse.token);
                tokenStorage.setRefreshToken(refreshResponse.refreshToken);

                originalRequest.headers.Authorization =
                    `Bearer ${refreshResponse.token}`;

                return api(originalRequest);
            }
            catch (refreshError) {
                tokenStorage.clearTokens();
                window.location.href = "/login";

                return Promise.reject(refreshError);
            }
        }
        return Promise.reject(error);
    }
)

export default api