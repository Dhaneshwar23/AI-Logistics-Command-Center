interface JwtPayload {
    exp?: number;
}

const decodeJwtPayload = (token: string): JwtPayload | null => {
    try {
        const payload = token.split(".")[1];

        if (!payload) {
            return null;
        }

        const normalized = payload
            .replace(/-/g, "+")
            .replace(/_/g, "/");

        const decoded = atob(normalized);

        return JSON.parse(decoded) as JwtPayload;
    } catch {
        return null;
    }
};

export const isTokenExpired = (token: string): boolean => {
    const payload = decodeJwtPayload(token);

    if (!payload?.exp) {
        return true;
    }

    return payload.exp * 1000 <= Date.now();
};