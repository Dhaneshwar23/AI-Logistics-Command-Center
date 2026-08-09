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
    IconButton
} from "@mui/material";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import type { PagedResult } from "@/types/pagination";

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
        <Paper>
            <TableContainer component={Paper} sx={{ width: '100%', overflowX: 'auto' }}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell sx={{ minWidth: 180 }}>Company</TableCell>
                            <TableCell sx={{ minWidth: 180 }}>Contact Person</TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell>Phone</TableCell>
                            <TableCell>City</TableCell>
                            <TableCell>State</TableCell>
                            <TableCell>Country</TableCell>
                            <TableCell sx={{ minWidth: 110 }}>Postal Code</TableCell>
                            <TableCell sx={{ minWidth: 220 }}>Address</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>

                    <TableBody>
                        {
                            customers.map((customer) => (
                                <TableRow key={customer.id}>
                                    <TableCell>{customer.companyName}</TableCell>
                                    <TableCell>{customer.contactPerson}</TableCell>
                                    <TableCell>{customer.email}</TableCell>
                                    <TableCell>{customer.phoneNumber}</TableCell>
                                    <TableCell>{customer.city}</TableCell>
                                    <TableCell>{customer.state}</TableCell>
                                    <TableCell>{customer.country}</TableCell>
                                    <TableCell>{customer.postalCode}</TableCell>
                                    <TableCell>{customer.address}</TableCell>
                                    <TableCell>{customer.isActive ? "Active" : "Inactive"}</TableCell>
                                    <TableCell align="right">
                                        <Tooltip title="Edit Customer">
                                            <IconButton size="small"
                                                onClick={() => onEdit(customer)}>
                                                <EditIcon />
                                            </IconButton>
                                        </Tooltip>

                                        <Tooltip title="Delete Customer">
                                            <IconButton size="small"
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
        </ Paper>
    )
}

export default CustomerTable;