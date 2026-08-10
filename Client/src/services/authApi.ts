import axios from "axios";

console.log("baseURL " + import.meta.env.VITE_API_BASE_URL);
const authApi = axios.create({
    
    baseURL: import.meta.env.VITE_API_BASE_URL,
    headers: {
        "Content-Type": "application/json",
    },
});

export default authApi;