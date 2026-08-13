import shipmentService from "@/services/shipmentService";
import type { PagedResult } from "@/types/pagination";
import type { CreateShipmentRequest, Shipment, UpdateShipmentRequest } from "@/types/shipment";
import getApiErrorMessage from "@/utils/getApiErrorMessage";
import { Alert, Box, Button, CircularProgress, Stack, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import AddIcon from "@mui/icons-material/Add"
import ShipmentTable from "@/components/shipments/ShipmentTable"
import type { Customer } from "@/types/customer";
import customerService from "@/services/customerService";
import ShipmentDialog from "@/components/shipments/ShipmentDialog";
import DeleteShipmentDialog from "@/components/shipments/DeleteShipmentDialog";

function ShipmentsPage() {
    const [pagedShipments, setPagedShipments] = useState<PagedResult<Shipment> | null>(null);
    const [customers, setCustomers] = useState<Customer[]>([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const [dialogOpen, setDialogOpen] = useState(false);
    const [dialogMode, setDialogMode] = useState<'create' | 'edit'>('create');
    const [selectedShipment, setSelectedShipment] = useState<Shipment | null>(null);
    const [mutationLoading, setMutationLoading] = useState(false);
    const [mutationError, setMutationError] = useState<string | null>(null);
    const [deleteOpen, setDeleteOpen] = useState(false);
    const [deleteLoading, setDeleteLoading] = useState(false);
    const [deleteError, setDeleteError] = useState<string | null>(null);
    const [shipmentToDelete, setShipmentToDelete] = useState<Shipment | null>(null);

    const shipments = pagedShipments?.items ?? [];

    const shipmentResponse = async () => {
        try {

            setLoading(true);
            setError(null);

            const res = await shipmentService.getAllShipments({ pageNumber, pageSize });

            setPagedShipments(res);

        }
        catch (error: unknown) {
            setError(
                getApiErrorMessage({ error, defaultMessage: "Unable to load Shipments." })
            );
        }
        finally {
            setLoading(false);
        }
    };

    const handleSubmitShipment = async (request: CreateShipmentRequest | UpdateShipmentRequest) => {
        try {
            setMutationLoading(true);
            setMutationError(null);

            if (dialogMode === 'edit') {
                if (!selectedShipment || !('rowVersion' in request)) return;
                await shipmentService.updateShipment(selectedShipment.id, request);
            } else {
                if (!('customerId' in request)) return;
                await shipmentService.createShipment(request);
            }

            setDialogOpen(false);
            setSelectedShipment(null);

            await shipmentResponse();
        }
        catch (error: unknown) {
            setMutationError(getApiErrorMessage({
                error,
                defaultMessage: dialogMode === 'edit' ? 'Unable to update shipment.' : 'Unable to create shipment.'
            }));
        }
        finally {
            setMutationLoading(false);
        }
    };

    const handleAddShipment = () => {
        setDialogMode('create');
        setSelectedShipment(null);
        setMutationError(null);
        setDialogOpen(true);
    };

    const handleEditShipment = (shipment: Shipment) => {
        setDialogMode('edit');
        setSelectedShipment(shipment);
        setMutationError(null);
        setDialogOpen(true);
    };

    const handleCloseDialog = () => {
        setDialogOpen(false);
        setSelectedShipment(null);
        setMutationError(null);
    };

    const handleDeleteClick = (shipment: Shipment) => {
        setShipmentToDelete(shipment);
        setDeleteError(null);
        setDeleteOpen(true);
    };

    const handleCloseDelete = () => {
        setDeleteOpen(false);
        setShipmentToDelete(null);
        setDeleteError(null);
    };

    const handleDeleteShipment = async () => {
        if (!shipmentToDelete) return;
        try {
            setDeleteLoading(true);
            setDeleteError(null);
            await shipmentService.deleteShipment(shipmentToDelete.id);
            setDeleteOpen(false);
            setShipmentToDelete(null);

            if (shipments.length === 1 && pageNumber > 1) {
                setPageNumber(pageNumber - 1);
            } else {
                await shipmentResponse();
            }
        } catch (error: unknown) {
            setDeleteError(getApiErrorMessage({ error, defaultMessage: 'Unable to delete shipment.' }));
        } finally {
            setDeleteLoading(false);
        }
    };

    const loadCustomers = async () => {
        try {
            const res = await customerService.getAllCustomers({
                pageNumber: 1,
                pageSize: 100
            });

            setCustomers(res.items);
        }
        catch (error: unknown) {
            setError(getApiErrorMessage({ error, defaultMessage: "Unable to find customers" }));
        }
    };

    const handlePageChange = (
        _event: unknown,
        newPage: number

    ) => {
        setPageNumber(newPage + 1);
    };

    const handlePageSizeChange = (event: React.ChangeEvent<HTMLInputElement>,
    ) => {
        setPageSize(Number(event.target.value))
        setPageNumber(1);
    }

    useEffect(() => {
        shipmentResponse();
    }, [pageNumber, pageSize]);

    useEffect(() => {
        loadCustomers();
    }, []);

    return (
        <Box>
            <Stack component="div"
                direction={{ xs: "column", sm: "row" }}
                sx={{ mb: 3, justifyContent: 'space-between', alignItems: { xs: "stretch", sm: "center" } }}>

                <Typography variant="h4" sx={{ mb: { xs: 2, sm: 0 } }}>
                    Shipments
                </Typography>

                <Button variant="contained" startIcon={<AddIcon />} onClick={handleAddShipment} sx={{ width: { xs: "100%", sm: "auto" } }}>
                    Add Shipment
                </Button>

            </Stack>

            {
                loading && (<Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}> <CircularProgress /> </Box>)
            }

            {!loading && !error && shipments.length === 0 && (
                <Alert severity="info">No shipments found.</Alert>
            )}

            {
                error && (<Alert severity="error" sx={{ mb: 2 }}>
                    {error}
                </Alert>)
            }

            {

                !loading && !error && shipments.length > 0 && (

                    <ShipmentTable
                        shipments={shipments}
                        count={pagedShipments?.totalCount ?? 0}
                        pageNumber={pageNumber}
                        rowsPerPage={pageSize}
                        onPageChange={handlePageChange}
                        onRowsPerPageChange={handlePageSizeChange}
                        onEdit={handleEditShipment}
                        onDelete={handleDeleteClick}

                    />
                )
            }

            <ShipmentDialog
                open={dialogOpen}
                customers={customers}
                loading={mutationLoading}
                mode={dialogMode}
                shipmentToEdit={selectedShipment}
                error={mutationError}
                onClose={handleCloseDialog}
                onSubmit={handleSubmitShipment}
            />
            <DeleteShipmentDialog
                open={deleteOpen}
                shipment={shipmentToDelete}
                loading={deleteLoading}
                error={deleteError}
                onClose={handleCloseDelete}
                onConfirm={handleDeleteShipment}
            />
        </Box>
    )
};

export default ShipmentsPage;

