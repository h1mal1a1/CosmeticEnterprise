import { apiRequest } from './client';

export type Category = {
  id: number;
  name: string;
};

export type CreateCategoryRequest = {
  name: string;
};

export type UpdateCategoryRequest = {
  name: string;
};

export function getCategories(): Promise<Category[]> {
  return apiRequest<Category[]>('/api/product-categories');
}

export function createCategory(request: CreateCategoryRequest): Promise<Category> {
  return apiRequest<Category>('/api/product-categories', {
    method: 'POST',
    body: request,
  });
}

export function updateCategory(id: number, request: UpdateCategoryRequest): Promise<void> {
  return apiRequest<void>(`/api/product-categories/${id}`, {
    method: 'PUT',
    body: request,
  });
}

export function deleteCategory(id: number): Promise<void> {
  return apiRequest<void>(`/api/product-categories/${id}`, {
    method: 'DELETE',
  });
}