import axios from "axios";

interface GetApiErrorMessageProps {
    error: unknown;
    defaultMessage?: string;
};

const getApiErrorMessage = ({
    error,
    defaultMessage = "An unexpected error occurred.",
}: GetApiErrorMessageProps): string => {
    if (axios.isAxiosError(error)) {
        if (error.response?.status === 401) {
            return "Your session has expired or you are not authorized. Please sign in again.";
        }

        if (error.response?.status === 403) {
            return "You do not have permission to perform this action.";
        }

        return (
            error.response?.data?.message ??
            defaultMessage
        );
    }

    return defaultMessage;
};

export default getApiErrorMessage;
