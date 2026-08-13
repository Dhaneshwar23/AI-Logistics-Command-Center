export interface Customer extends CustomerBaseModel {
    id: number;
    isActive: boolean;
    rowVersion: string;
}

export interface CreateCustomerRequest extends CustomerBaseModel { }

export interface UpdateCustomerRequest extends CustomerBaseModel {
    rowVersion: string;
}

export interface CustomerBaseModel {
    companyName: string;
    contactPerson: string;
    email: string;
    phoneNumber: string;
    address: string;
    city: string;
    state: string;
    country: string;
    postalCode: string;
}

export const emptyCustomer: CustomerBaseModel = {
    companyName: '',
    contactPerson: '',
    email: '',
    phoneNumber: '',
    address: '',
    city: '',
    state: '',
    country: '',
    postalCode: '',
}

export interface UpdateCustomerRequest  extends CustomerBaseModel{
    rowVersion: string;
}

export type CustomerValidationErrors = Partial<Record<keyof CustomerBaseModel, string>>