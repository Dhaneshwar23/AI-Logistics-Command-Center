import type { Customer, CustomerBaseModel, CustomerValidationErrors } from '@/types/customer';
import customerService from '@/services/customerService';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    Alert,
    useMediaQuery,
    useTheme
} from '@mui/material';
import { useEffect, useState } from 'react';
import CustomerForm from './CustomerForm';
import { emptyCustomer } from '@/types/customer';
import getApiErrorMessage from '@/utils/getApiErrorMessage';


interface CustomerDialogProps {
    open: boolean;
    onClose: () => void;
    onSuccess: () => void;
    mode: 'create' | 'edit';
    customerToEdit: Customer | null;
}

const CustomerDialog = ({
    open,
    onClose,
    onSuccess,
    mode,
    customerToEdit
}: CustomerDialogProps) => {
    const [customer, setCustomer] = useState<CustomerBaseModel>(emptyCustomer);
    const [validationErrors, setValidationErrors] = useState<CustomerValidationErrors>({});
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const validateCustomer = () => {
        const errors: CustomerValidationErrors = {}
        if (!customer.companyName.trim()) {
            errors.companyName = "Company Name is required.";
        }
        if (!customer.contactPerson.trim()) {
            errors.contactPerson = "Contact Person is required.";
        }
        if (!customer.email.trim()) {
            errors.email = "Email is required.";
        }
        if (!customer.phoneNumber.trim()) {
            errors.phoneNumber = "Phone Number is required.";
        }
        if (!customer.city.trim()) {
            errors.city = "City is required.";
        }
        if (!customer.state.trim()) {
            errors.state = "State is required.";
        }
        if (!customer.country.trim()) {
            errors.country = "Country is required.";
        }
        if (!customer.postalCode.trim()) {
            errors.postalCode = "Postal Code is required.";
        }
        if (!customer.address.trim()) {
            errors.address = "Address is required.";
        }

        setValidationErrors(errors);

        return Object.keys(errors).length === 0;
    }
    const handleFieldChange = (
        field: keyof CustomerBaseModel,
        value: string
    ) => {
        setCustomer((previous) => ({
            ...previous,
            [field]: value,
        }));

        setValidationErrors((previous) => ({
            ...previous,
            [field]: undefined,
        }));
    };
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async () => {
        if (!validateCustomer()) {
            return;
        }
        try {
            setIsSubmitting(true);
            setError(null);

            if (mode === 'create') {
                await customerService.createCustomer(customer);

                setCustomer(emptyCustomer);
                setValidationErrors({});
                onSuccess();
                onClose();
            }
            else {
                if (!customerToEdit) {
                    return;
                }

                await customerService.updateCustomer(customerToEdit.id, {
                    ...customer,
                    rowVersion: customerToEdit.rowVersion
                }
                );

                setCustomer(emptyCustomer);
                setValidationErrors({});
                onSuccess();
                onClose();
            }

        }
        catch (error: unknown) {
            setError(
                getApiErrorMessage({ error, defaultMessage: 'Unable to create customer.' })
            );
        }
        finally {
            setIsSubmitting(false);
        }
    }

    const handleCancel = () => {
        setCustomer(emptyCustomer);
        setError(null);
        onClose();
    }

    useEffect(() => {
        if (mode === 'edit' && customerToEdit) {
            setCustomer({
                companyName: customerToEdit.companyName,
                contactPerson: customerToEdit.contactPerson,
                email: customerToEdit.email,
                phoneNumber: customerToEdit.phoneNumber,
                address: customerToEdit.address,
                city: customerToEdit.city,
                state: customerToEdit.state,
                country: customerToEdit.country,
                postalCode: customerToEdit.postalCode,
            });
        }
        else {
            setCustomer(emptyCustomer);
        }

        setValidationErrors({});
        setError(null);
    }, [mode, customerToEdit]);


    return (
        <Dialog open={open}
                onClose={onClose}
                fullWidth
                maxWidth="md"
                fullScreen={isMobile}>
            <DialogTitle>{mode === 'edit' ? 'Edit Customer' : 'Add Customer'}</DialogTitle>
            <DialogContent sx={{ pt: 2.5 }}>
                {error && (
                    <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>
                )}
                <CustomerForm values={customer} errors={validationErrors} onChange={handleFieldChange} />
            </DialogContent>
            <DialogActions>
                <Button
                    variant="contained"
                    onClick={handleCancel}
                    disabled={isSubmitting}
                >
                    Cancel
                </Button>

                <Button variant="contained" onClick={handleSubmit} disabled={isSubmitting}>
                    {isSubmitting ? mode === 'create'
                        ? 'Adding...'
                        : 'Saving...'
                        : mode === 'create'
                            ? 'Add Customer'
                            : 'Save Changes'}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default CustomerDialog;