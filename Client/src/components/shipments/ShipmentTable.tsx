import type { Shipment } from "@/types/shipment";
import {
    getShipmentStatusLabel,
    getPaymentStatusLabel,
    getPaymentStatusColor,
    getShipmentStatusColor
} from "@/utils/shipmentStatus";

import {
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TablePagination,
    Tooltip,
    IconButton
}
    from "@mui/material";
import Chip from "@mui/material/Chip";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

interface ShipmentTableProps {
    shipments: Shipment[];
    count: number;
    rowsPerPage: number;
    pageNumber: number;
    onEdit: (shipment: Shipment) => void;
    onDelete: (shipment: Shipment) => void;
    onPageChange: (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => void;
    onRowsPerPageChange: React.ChangeEventHandler<HTMLTextAreaElement | HTMLInputElement>;
}

const ShipmentTable = ({
    shipments,
    count,
    rowsPerPage,
    pageNumber,
    onEdit,
    onDelete,
    onPageChange,
    onRowsPerPageChange
}: ShipmentTableProps) => {

    return (
        <Paper variant="outlined" sx={{ overflow: 'hidden' }}>
            <TableContainer sx={{ width: '100%', overflowX: 'auto' }}>
                <Table size="small" sx={{ minWidth: 980 }}>
                    <TableHead>
                        <TableRow>
                            <TableCell>Shipment #</TableCell>
                            <TableCell>Customer</TableCell>
                            <TableCell>Origin</TableCell>
                            <TableCell>Destination</TableCell>
                            <TableCell>Weight</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Payment</TableCell>
                            <TableCell align="right">Actions</TableCell>
                        </TableRow>
                    </TableHead>

                    <TableBody>
                        {
                            shipments.map((shipment) =>
                                <TableRow key={shipment.id} hover>
                                    <TableCell sx={{ minWidth: 190, maxWidth: 240, fontWeight: 700, color: 'primary.dark', overflowWrap: 'anywhere' }}>{shipment.shipmentNumber}</TableCell>
                                    <TableCell sx={{ minWidth: 160, maxWidth: 230, overflowWrap: 'anywhere' }}>{shipment.customerName}</TableCell>
                                    <TableCell>{shipment.origin}</TableCell>
                                    <TableCell>{shipment.destination}</TableCell>
                                    <TableCell>{shipment.weightKg}</TableCell>
                                    <TableCell>
                                        <Chip
                                            label={getShipmentStatusLabel(shipment.shipmentStatus)}
                                            color={getShipmentStatusColor(shipment.shipmentStatus)}
                                            variant="outlined"
                                            size="small" />
                                    </TableCell>
                                    <TableCell>
                                        <Chip
                                            label={getPaymentStatusLabel(shipment.paymentStatus)}
                                            color={getPaymentStatusColor(shipment.paymentStatus)}
                                            variant="outlined"
                                            size="small" />
                                    </TableCell>
                                    <TableCell align="right" sx={{ whiteSpace: 'nowrap' }}>
                                        <Tooltip title="Edit Shipment">
                                            <IconButton size="small" aria-label={`Edit ${shipment.shipmentNumber}`} onClick={() => onEdit(shipment)}><EditIcon /></IconButton>
                                        </Tooltip>
                                        <Tooltip title="Delete Shipment">
                                            <IconButton size="small" color="error" aria-label={`Delete ${shipment.shipmentNumber}`} onClick={() => onDelete(shipment)}><DeleteIcon /></IconButton>
                                        </Tooltip>
                                    </TableCell>
                                </TableRow>
                            )
                        }
                    </TableBody>
                </Table>
            </TableContainer>

            <TablePagination
                component="div"
                count={count}
                page={pageNumber - 1}
                rowsPerPage={rowsPerPage}
                onPageChange={onPageChange}
                onRowsPerPageChange={onRowsPerPageChange}
                rowsPerPageOptions={[5, 10, 25]} />
        </Paper>
    )
};

export default ShipmentTable;
