import { apiRequest } from './client';
import type { AuthResponse, CurrentUser, LoginRequest } from '../types/auth';

export function login(request: LoginRequest): Promise<AuthResponse> {
  return apiRequest<AuthResponse>('/api/auth/login', {
    method: 'POST',
    body: request,
  });
}

export function getCurrentUser(token: string): Promise<CurrentUser> {
  return apiRequest<CurrentUser>('/api/auth/me', {
    token,
  });
}