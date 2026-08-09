import React, { useState, useEffect } from 'react';
import type { Customer } from '@/types/customer';
import type { PagedResult } from '@/types/pagination';
import customerService from '@/services/customerService';
import CustomerDialog from '@/components/customers/CustomerDialog';
import DeleteCustomersDialog from '@/components/customers/DeleteCustomersDialog';
import axios from 'axios';
import {
    Stack,
    Alert,
    Box,
    CircularProgress,
    Button,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TablePagination,
    TableRow,
    TextField,
    Typography,
    IconButton,
    Tooltip,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import CustomerTable from '@/components/customers/CustomerTable';
import getApiErrorMessage from '@/utils/getApiErrorMessage';


function CustomersPage() {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [pagedCustomers, setPagedCustomers] = useState<PagedResult<Customer> | null>(null);
    const [pageNumber, setPageNumber] = useState<number>(1);
    const [pageSize, setPageSize] = useState<number>(10);
    const [isDialogOpen, setIsDialogOpen] = useState<boolean>(false);
    const [selectedCustomer, setSelectedCustomer] = useState<Customer | null>(null);
    const [dialogMode, setDialogMode] = useState<'create' | 'edit'>('create');
    const [customerToDelete, setCustomerToDelete] = useState<Customer | null>(null);
    const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState<boolean>(false);

    const customers = pagedCustomers?.items ?? [];

    const customerResponse = async () => {
        try {

            setLoading(true);
            setError(null);

            const res = await customerService.getAllCustomers({ pageNumber, pageSize });

            setPagedCustomers(res);
        }
        catch (error: unknown) {
            setError(
                getApiErrorMessage({ error, defaultMessage: 'Unable to load customers.' })
            );
        }
        finally {
            setLoading(false);
        }
    }

    const handlePageChange = (
        _event: unknown,
        newPage: number
    ) => {
        setPageNumber(newPage + 1);
    }

    const handlePageSizeChange = (
        event: React.ChangeEvent<HTMLInputElement>,
    ) => {
        setPageSize(Number(event.target.value))
        setPageNumber(1);
    }

    const handleOpenDialog = () => {
        console.log("Open dialog");
        setIsDialogOpen(true);
    }

    const handleCloseDialog = () => {
        console.log("Close dialog");
        setIsDialogOpen(false);
    }

    const handleEditCustomer = (customer: Customer) => {
        setSelectedCustomer(customer);
        setIsDialogOpen(true);
    }

    const handleDeleteCustomer = (customer: Customer) => {
        setCustomerToDelete(customer);
        setIsDeleteDialogOpen(true);
    }

    const handleAddCustomer = () => {
        setDialogMode('create');
        setSelectedCustomer(null);
        setIsDialogOpen(true);
    };

    const handleEditCustomerClick = (customer: Customer) => {
        setDialogMode('edit');
        setSelectedCustomer(customer);
        setIsDialogOpen(true);
    };


    useEffect(() => {
        customerResponse();
    }, [pageNumber, pageSize]);

    if (loading) {
        return <p>Loading customers...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    return (

        <Box>
            <Stack
                component="div"
                direction="row"
                sx={{ mb: 3, justifyContent: 'space-between', alignItems: 'center' }}
            >
                <Typography variant="h4" sx={{ mb: 3 }}>
                    Customers
                </Typography>

                <Button variant="contained" startIcon={<AddIcon />} onClick={handleAddCustomer}>
                    Add Customer
                </Button>
            </Stack>

            {
                loading && (<Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}> <CircularProgress /> </Box>)
            }

            {
                error && (<Alert severity="error" sx={{ mb: 2 }}>
                    {error}
                </Alert>)
            }

            {
                !loading && !error && customers.length === 0 && (
                    <Alert severity="info">
                        No customers found.
                    </Alert>
                )
            }

            {
                !loading && !error && customers.length > 0 && (
                    <CustomerTable
                        customers={customers}
                        onEdit={handleEditCustomerClick}
                        onDelete={handleDeleteCustomer}
                        count={pagedCustomers?.totalCount ?? 0}
                        pageNumber={pageNumber}
                        rowsPerPage={pageSize}
                        onPageChange={handlePageChange}
                        onRowsPerPageChange={handlePageSizeChange}

                    />
                )
            }
            <CustomerDialog
                open={isDialogOpen}
                onClose={handleCloseDialog}
                onSuccess={customerResponse}
                mode={dialogMode}
                customerToEdit={selectedCustomer}
            />
            <DeleteCustomersDialog
                open={isDeleteDialogOpen}
                customer={customerToDelete}
                onSuccess={customerResponse}
                onClose={() => setIsDeleteDialogOpen(false)}
            />
        </Box>
    )
}

export default CustomersPage;
