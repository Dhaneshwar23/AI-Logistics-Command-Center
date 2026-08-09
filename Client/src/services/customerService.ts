import api from "@/services/api";
import type {
    Customer,
    CreateCustomerRequest,
    UpdateCustomerRequest,
    GetCustomersRequest
} from "@/types/customer";
import type { PagedResult } from "@/types/pagination"

const customerService = {
    getAllCustomers: async (
        request: GetCustomersRequest)
        : Promise<PagedResult<Customer>> => {
        const res = await api.get<PagedResult<Customer>>(
            "/api/v1/customers",
            {
                params: request,
            }
        );

        return res.data;
    },
    createCustomer: async (
        request: CreateCustomerRequest):
        Promise<Customer> => {
        const response = await api.post<Customer>(
            '/api/v1/customers',
            request
        )

        return response.data;
    },
    updateCustomer: async (
        id: number,
        request: UpdateCustomerRequest):
        Promise<Customer> => {
        const response = await api.put<Customer>(
            `/api/v1/customers/${id}`,
            request
        )

        return response.data;
    },
    deleteCustomer: async(id: number):
    Promise<void> => {
        await api.delete(`/api/v1/customers/${id}`);
    }

};

export default customerService;


