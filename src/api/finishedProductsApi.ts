import { apiRequest } from "./client";
import type {
    FinishedProduct, 
    CreateFinishedProductRequest,
    UpdateFinishedProductRequest,
} from '../types/finishedProduct';

export function getFinishedProducts(): Promise<FinishedProduct[]> {
    return apiRequest<FinishedProduct[]>('/api/finished-products');
}

export function getFinishedProductById(
    id: number,
): Promise<FinishedProduct> {
    return apiRequest<FinishedProduct>(`/api/finished-products/${id}`);
}

export function createFinishedProduct(
    request: CreateFinishedProductRequest,
): Promise<FinishedProduct> {
    return apiRequest<FinishedProduct>('/api/finished-products', {
        method: 'POST',
        body: request
    });
}

export function updateFinishedProduct(
    id: number,
    request: UpdateFinishedProductRequest,
): Promise<FinishedProduct> {
    return apiRequest<FinishedProduct>(`/api/finished-products/${id}`, {
        method: 'PUT',
        body: request
    });
}

export function deleteFinishedProduct(
    id: number,
): Promise<void> {
    return apiRequest<void>(`/api/finished-products/${id}`, {
        method: 'DELETE'
    });
}