import axios from "axios";

const authApi = axios.create({
    baseURL: "https://ai-logistics-api-dk-dqbxfmezfaafc2gn.southeastasia-01.azurewebsites.net",
    headers: {
        "Content-Type": "application/json",
    },
});

export default authApi;