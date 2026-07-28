let accessToken : string | null = null

const getAccessToken = () => accessToken

const setAccessToken = (token:string) => {
    accessToken = token
}

const clearAccessToken = () =>{
    accessToken = null
}

export default{
    getAccessToken,
    setAccessToken,
    clearAccessToken
}