import type { Customer } from "@/types/customer";
import { useEffect, useState } from "react";
import {
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    TextField,
    FormHelperText,
    Box
} from "@mui/material";
import type { CreateShipmentRequest, Shipment, UpdateShipmentRequest } from "@/types/shipment";

interface ShipmentFormProps {
    customers: Customer[];
    open: boolean;
    mode: 'create' | 'edit';
    shipmentToEdit: Shipment | null;
    onSubmit: (request: CreateShipmentRequest | UpdateShipmentRequest) => Promise<void>;
    loading: boolean;
}

const toDateTimeLocal = (value: string): string => {
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '';
    const offset = date.getTimezoneOffset() * 60_000;
    return new Date(date.getTime() - offset).toISOString().slice(0, 16);
};

const ShipmentForm = ({ customers, open, mode, shipmentToEdit,
    onSubmit,
    loading
}: ShipmentFormProps) => {
    const [customerId, setCustomerId] = useState<number | "">("");
    const [origin, setOrigin] = useState("");
    const [destination, setDestination] = useState("");
    const [weightKg, setWeightKg] = useState<number | "">("");
    const [pickupDate, setPickupDate] = useState("");
    const [deliveryDate, setDeliveryDate] = useState("");
    const [customerError, setCustomerError] = useState("");
    const [originError, setOriginError] = useState("");
    const [destinationError, setDestinationError] = useState("");
    const [weightError, setWeightError] = useState("");
    const [pickupDateError, setPickupError] = useState("");
    const [deliveryDateError, setDeliveryDateError] = useState("");

    useEffect(() => {
        if (!open) return;
        if (mode === 'edit' && shipmentToEdit) {
            setCustomerId(shipmentToEdit.customerId);
            setOrigin(shipmentToEdit.origin);
            setDestination(shipmentToEdit.destination);
            setWeightKg(shipmentToEdit.weightKg);
            setPickupDate(toDateTimeLocal(shipmentToEdit.pickupDate));
            setDeliveryDate(toDateTimeLocal(shipmentToEdit.deliveryDate));
        } else {
            setCustomerId('');
            setOrigin('');
            setDestination('');
            setWeightKg('');
            setPickupDate('');
            setDeliveryDate('');
        }
        setCustomerError('');
        setOriginError('');
        setDestinationError('');
        setWeightError('');
        setPickupError('');
        setDeliveryDateError('');
    }, [open, mode, shipmentToEdit]);

    const validateForm = (): boolean => {
        let isValid = true;

        setCustomerError("");
        setOriginError("");
        setDestinationError("");
        setWeightError("");
        setPickupError("");
        setDeliveryDateError("");

        const pickup = new Date(pickupDate);
        const delivery = new Date(deliveryDate);

        const isPickupValid = !isNaN(pickup.getTime());
        const isDeliveryValid = !isNaN(delivery.getTime());

        if (mode === 'create' && customerId === "") {
            setCustomerError("Customer is required");
            isValid = false;
        }

        if (!origin.trim()) {
            setOriginError("Origin is required");
            isValid = false;
        }

        if (origin.trim() &&
            destination.trim() &&
            origin.trim().toLowerCase() === destination.trim().toLowerCase()
        ) {
            setDestinationError("Destination must be different from origin");
            isValid = false;
        }

        if (!destination.trim()) {
            setDestinationError("Destionation is required");
            isValid = false;
        }

        if (weightKg === "" || weightKg <= 0) {
            setWeightError("Weight must be greater than 0");
            isValid = false;
        }

        if (!pickupDate || isNaN(new Date(pickupDate).getTime())) {
            setPickupError("Valid pickup date and time is required");
            isValid = false;
        }

        if (!deliveryDate || isNaN(new Date(deliveryDate).getTime())) {
            setDeliveryDateError("Valid delivery date and time is required");
            isValid = false;
        }

        if (
            isPickupValid &&
            isDeliveryValid &&
            delivery <= pickup
        ) {
            setDeliveryDateError("Delivery date must be after pickup date");
            isValid = false;
        }

        return isValid;
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!validateForm()) {
            return;
        }

        const editableValues = {
            origin: origin.trim(),
            destination: destination.trim(),
            weightKg: Number(weightKg),
            pickupDate: new Date(pickupDate).toISOString(),
            deliveryDate: new Date(deliveryDate).toISOString(),
        };

        if (mode === 'edit' && shipmentToEdit) {
            await onSubmit({ ...editableValues, rowVersion: shipmentToEdit.rowVersion });
        } else {
            await onSubmit({ ...editableValues, customerId: Number(customerId) });
        }
    }

    return (
        <Box
            component="form"
            id="shipment-form"
            onSubmit={handleSubmit}
            noValidate
        >
            {mode === 'create' && <FormControl fullWidth margin="normal" error={Boolean(customerError)}>
                <InputLabel id="customer-label">Customer</InputLabel>

                <Select
                    labelId="customer-label"
                    value={customerId}
                    label="customer"
                    disabled={loading}
                    onChange={
                        (e) => {
                            const value = e.target.value;
                            setCustomerId(String(value) === '' ? '' : Number(value));
                            setCustomerError("");
                        }
                    }
                >

                    <MenuItem value="">
                        <em>Select Customer</em>
                    </MenuItem>

                    {customers.map((customer) => (
                        <MenuItem
                            key={customer.id}
                            value={customer.id}
                        >
                            {customer.companyName}
                        </MenuItem>
                    ))}
                </Select>

                <FormHelperText>{customerError}</FormHelperText>
            </FormControl>}

            <TextField
                label="Origin"
                value={origin}
                onChange={(e) => { setOrigin(e.target.value); setOriginError(""); }}
                error={Boolean(originError)}
                helperText={originError}
                fullWidth
                margin="normal"
                disabled={loading}
            />

            <TextField
                label="Destination"
                value={destination}
                onChange={
                    (e) => {
                        setDestination(e.target.value);
                        setDestinationError("");
                    }
                }
                error={Boolean(destinationError)}
                helperText={destinationError}
                fullWidth
                margin="normal"
                disabled={loading}
            />
            <TextField
                label="Weight (Kg)"
                value={weightKg}
                onChange={(e) => {
                    const value = e.target.value;
                    setWeightKg(value === "" ? "" : Number(value));
                    setWeightError("")
                }
                }
                error={Boolean(weightError)}
                helperText={weightError}
                type="number"
                fullWidth
                margin="normal"
                disabled={loading}
                slotProps={{ htmlInput: { min: 0.01, max: 1000000, step: 'any' } }}
            />

            <TextField
                label="Pickup Date"
                type="datetime-local"
                value={pickupDate}
                onChange={(e) => {
                    setPickupDate(e.target.value);
                    setPickupError("");
                }
                }
                error={Boolean(pickupDateError)}
                helperText={pickupDateError}
                fullWidth
                margin="normal"
                disabled={loading}
                slotProps={{
                    inputLabel: {
                        shrink: true,
                    },
                }}
            />

            <TextField
                label="Delivery Date"
                type="datetime-local"
                value={deliveryDate}
                onChange={(e) => {
                    setDeliveryDate(e.target.value);
                    setDeliveryDateError("");

                }}
                error={Boolean(deliveryDateError)}
                helperText={deliveryDateError}
                fullWidth
                margin="normal"
                disabled={loading}
                slotProps={{
                    inputLabel: {
                        shrink: true,
                    },
                }}
            />

        </Box>
    )
}

export default ShipmentForm;
