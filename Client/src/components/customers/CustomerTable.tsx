import type { Customer } from "@/types/customer";
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
    IconButton,
    Chip
} from "@mui/material";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

interface CustomerTableProps {
    customers: Customer[];
    onEdit: (customer: Customer) => void;
    onDelete: (customer: Customer) => void;
    count: number;
    rowsPerPage: number;
    pageNumber: number;
    onPageChange: (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => void;
    onRowsPerPageChange: React.ChangeEventHandler<HTMLTextAreaElement | HTMLInputElement>;
}

const CustomerTable = ({
    customers,
    onEdit,
    onDelete,
    count,
    rowsPerPage,
    pageNumber,
    onPageChange,
    onRowsPerPageChange
}: CustomerTableProps) => {
    return (
        <Paper variant="outlined" sx={{ overflow: 'hidden' }}>
            <TableContainer sx={{ width: '100%', overflowX: 'auto' }}>
                <Table size="small" sx={{ minWidth: 980 }}>
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ minWidth: 180 }}>Company</TableCell>
                            <TableCell sx={{ minWidth: 180 }}>Contact Person</TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell>Phone</TableCell>
                            <TableCell>City</TableCell>
                            <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>State</TableCell>
                            <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>Country</TableCell>
                            <TableCell sx={{ display: { xs: "none", md: "table-cell" },minWidth: 110 }}>Postal Code</TableCell>
                            <TableCell sx={{ display: { xs: "none", md: "table-cell"}, minWidth: 220 }}>Address</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>

                    <TableBody>
                        {
                            customers.map((customer) => (
                                <TableRow key={customer.id} hover>
                                    <TableCell sx={{ fontWeight: 650, color: 'text.primary' }}>{customer.companyName}</TableCell>
                                    <TableCell>{customer.contactPerson}</TableCell>
                                    <TableCell>{customer.email}</TableCell>
                                    <TableCell>{customer.phoneNumber}</TableCell>
                                    <TableCell>{customer.city}</TableCell>
                                    <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>{customer.state}</TableCell>
                                    <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>{customer.country}</TableCell>
                                    <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>{customer.postalCode}</TableCell>
                                    <TableCell sx={{ display: { xs: "none", md: "table-cell" } }}>{customer.address}</TableCell>
                                    <TableCell><Chip label={customer.isActive ? "Active" : "Inactive"} color={customer.isActive ? 'success' : 'default'} size="small" variant="outlined" /></TableCell>
                                    <TableCell align="right">
                                        <Tooltip title="Edit Customer">
                                            <IconButton size="small" aria-label={`Edit ${customer.companyName}`}
                                                onClick={() => onEdit(customer)}>
                                                <EditIcon />
                                            </IconButton>
                                        </Tooltip>

                                        <Tooltip title="Delete Customer">
                                            <IconButton size="small" color="error" aria-label={`Delete ${customer.companyName}`}
                                                onClick={() => onDelete(customer)}>
                                                <DeleteIcon />
                                            </IconButton>
                                        </Tooltip>
                                    </TableCell>
                                </TableRow>
                            ))
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
}

export default CustomerTable;
