import { apiRequest } from './client';
import type { CurrentUser, LoginRequest } from '../types/auth';

export function login(request: LoginRequest): Promise<void> {
  return apiRequest<void>('/api/auth/login', {
    method: 'POST',
    body: request,
  });
}

export function getCurrentUser(): Promise<CurrentUser> {
  return apiRequest<CurrentUser>('/api/auth/me');
}

export function apiLogout(): Promise<void> {
  return apiRequest<void>('/api/auth/logout', {
    method: 'POST',
  });
}