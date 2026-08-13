import type { Customer } from "@/types/customer";
import customerService from "@/services/customerService";
import {
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Button,
    Typography,
    Alert
} from "@mui/material";
import { useState } from "react";
import getApiErrorMessage from "@/utils/getApiErrorMessage";

interface DeleteCustomersDialogProps {
    open: boolean;
    customer: Customer | null;
    onClose: () => void;
    onSuccess: () => void;
}

const DeleteCustomersDialog = ({
    open,
    customer,
    onClose,
    onSuccess
}: DeleteCustomersDialogProps) => {

    const [isDeleting, setIsDeleting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleDelete = async () => {
        if (!customer) {
            return;
        }
        try {
            setIsDeleting(true);
            setError(null);
            await customerService.deleteCustomer(customer.id);
            onSuccess();
            onClose();
        }
        catch (error: unknown) {
            setError(
                getApiErrorMessage({ error, defaultMessage: 'Unable to delete customer.' })
            );
        }
        finally {
            setIsDeleting(false);
        }

    }

    return (
        <Dialog open={open} onClose={isDeleting ? undefined : onClose} fullWidth maxWidth="xs">
            <DialogTitle>Delete Customer</DialogTitle>
            <DialogContent>
                {error && (
                    <Alert severity="error" sx={{ mb: 2 }}>
                        {error}
                    </Alert>
                )}
                <Typography>
                    Are you sure you want to delete
                    <strong> {customer?.companyName} </strong>?
                </Typography>
            </DialogContent>
            <DialogActions>
                <Button variant="outlined" onClick={onClose} disabled={isDeleting}>Cancel</Button>
                <Button color="error" variant="contained" onClick={handleDelete} disabled={isDeleting}>
                    {isDeleting ? "Deleting..." : "Delete"}
                </Button>
            </DialogActions>
        </Dialog>
    )
}

export default DeleteCustomersDialog;
