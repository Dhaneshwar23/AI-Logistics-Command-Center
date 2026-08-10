import type { CustomerBaseModel, CustomerValidationErrors } from '@/types/customer';
import {
    Grid,
    TextField,
} from '@mui/material';

interface CustomerFormProps {
    values: CustomerBaseModel
    errors: CustomerValidationErrors
    onChange: (
        field: keyof CustomerBaseModel,
        value: string
    ) => void
}

const CustomerForm = ({
    values,
    errors,
    onChange,
}: CustomerFormProps) => {
    return (
        <Grid container spacing={2} sx={{ mt: 0.5 }}>
            <Grid size={{ xs: 12, sm: 6 }}>
                <TextField
                    label="Company Name"
                    value={values.companyName}
                    onChange={(event) => onChange('companyName', event.target.value)}
                    error = {Boolean(errors.companyName)}
                    helperText = {errors.companyName}
                    fullWidth
                    required
                />
            </Grid>
            <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                    label="Contact Person"
                    value={values.contactPerson}
                    onChange={(event) =>
                        onChange('contactPerson', event.target.value)
                    }
                    error = {Boolean(errors.contactPerson)}
                    helperText = {errors.contactPerson}
                    fullWidth
                    required
                />
            </Grid>

            <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                    label="Email"
                    type="email"
                    value={values.email}
                    onChange={(event) =>
                        onChange('email', event.target.value)
                    }
                    error = {Boolean(errors.email)}
                    helperText = {errors.email}
                    fullWidth
                    required
                />
            </Grid>

            <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                    label="Phone Number"
                    value={values.phoneNumber}
                    onChange={(event) =>
                        onChange('phoneNumber', event.target.value)
                    }
                    error = {Boolean(errors.phoneNumber)}
                    helperText = {errors.phoneNumber}
                    fullWidth
                    required
                />
            </Grid>

            <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                    label="City"
                    value={values.city}
                    onChange={(event) =>
                        onChange('city', event.target.value)
                    }
                    error = {Boolean(errors.city)}
                    helperText = {errors.city}
                    fullWidth
                    required
                />
            </Grid>

            <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                    label="State"
                    value={values.state}
                    onChange={(event) =>
                        onChange('state', event.target.value)
                    }
                    error = {Boolean(errors.state)}
                    helperText = {errors.state}
                    fullWidth
                    required
                />
            </Grid>

            <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                    label="Country"
                    value={values.country}
                    onChange={(event) =>
                        onChange('country', event.target.value)
                    }
                    error = {Boolean(errors.country)}
                    helperText = {errors.country}
                    fullWidth
                    required
                />
            </Grid>

            <Grid size={{ xs: 12, md: 6 }}>
                <TextField
                    label="Postal Code"
                    value={values.postalCode}
                    onChange={(event) =>
                        onChange('postalCode', event.target.value)
                    }
                    error = {Boolean(errors.postalCode)}
                    helperText = {errors.postalCode}
                    fullWidth
                    required
                />
            </Grid>

            <Grid size={{ xs: 12 }}>
                <TextField
                    label="Address"
                    value={values.address}
                    onChange={(event) =>
                        onChange('address', event.target.value)
                    }
                    error = {Boolean(errors.address)}
                    helperText = {errors.address}
                    fullWidth
                    required
                    multiline
                    minRows={2}
                />
            </Grid>
        </Grid>
        // <Stack spacing={2} sx={{ mt: 1 }}>
        //     <TextField
        //         label="Company Name"
        //         value={values.companyName}
        //         onChange={(event) => onChange('companyName', event.target.value)}
        //         fullWidth
        //         required
        //     />
        //     <TextField
        //         label="Contact Person"
        //         value={values.contactPerson}
        //         onChange={(event) => onChange('contactPerson', event.target.value)}
        //         fullWidth
        //         required
        //     />
        // </Stack>
    )
}

export default CustomerForm;
