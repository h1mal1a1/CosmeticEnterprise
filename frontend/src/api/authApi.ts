import { apiRequest } from './client';
import type {
  CurrentUser,
  LoginRequest,
  RegisterRequest,
  UpdateProfileRequest,
} from '../types/auth';

export function login(request: LoginRequest): Promise<void> {
  return apiRequest<void>('/api/auth/login', {
    method: 'POST',
    body: request,
  });
}

export function register(request: RegisterRequest): Promise<void> {
  return apiRequest<void>('/api/auth/register', {
    method: 'POST',
    body: request,
  });
}

export function getCurrentUser(): Promise<CurrentUser> {
  return apiRequest<CurrentUser>('/api/auth/me');
}

export function updateProfile(
  request: UpdateProfileRequest
): Promise<CurrentUser> {
  return apiRequest<CurrentUser>('/api/auth/profile', {
    method: 'PUT',
    body: request,
  });
}

export function apiLogout(): Promise<void> {
  return apiRequest<void>('/api/auth/logout', {
    method: 'POST',
  });
}