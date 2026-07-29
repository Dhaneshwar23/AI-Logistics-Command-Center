import axios from 'axios'
import tokenStorage from './tokenStorage'

const api = axios.create({
    //baseURL: 'https://localhost:7009',
    baseURL:'https://ai-logistics-api-dk-dqbxfmezfaafc2gn.southeastasia-01.azurewebsites.net',
    headers: {
        'Content-Type': 'application/json',
    },
})

api.interceptors.request.use((config) => {
    const token = tokenStorage.getAccessToken()

    if(token)
    {
        config.headers.Authorization = `Bearer ${token}`
    }

    return config
})

export default api