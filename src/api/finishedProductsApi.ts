import { apiRequest } from "./client";
import type {
    FinishedProduct, 
    CreateFinishedProductRequest,
    UpdateFinishedProductRequest,
} from '../types/finishedProduct';

export function getProducts(token?: string): Promise<FinishedProduct[]> {
    return apiRequest<FinishedProduct[]>('/api/finished-products', {
        token,
    });
}

export function getProductById(
    id: number,
    token?: string
): Promise<FinishedProduct> {
    return apiRequest<FinishedProduct>(`/api/finished-products/${id}`, {
        token,
    });
}

export function createProduct(
    request: CreateFinishedProductRequest,
    token?: string
): Promise<FinishedProduct> {
    return apiRequest<FinishedProduct>('/api/finished-products', {
        method: 'POST',
        body: request,
        token,
    });
}

export function updateProduct(
    id: number,
    request: UpdateFinishedProductRequest,
    token?: string
): Promise<FinishedProduct> {
    return apiRequest<FinishedProduct>(`/api/finished-products/${id}`, {
        method: 'PUT',
        body: request,
        token,
    });
}

export function deleteProduct(
    id: number,
    token?: string
): Promise<void> {
    return apiRequest<void>(`/api/finished-products/${id}`, {
        method: 'DELETE',
        token,
    });
}