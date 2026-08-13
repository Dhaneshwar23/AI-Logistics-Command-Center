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
        return (
            error.response?.data?.message ??
            defaultMessage
        );
    }

    return defaultMessage;
};

export default getApiErrorMessage;
