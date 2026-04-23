import { apiRequest } from "./client";
import type {
  CreateUserAddressRequest,
  UpdateUserAddressRequest,
  UserAddress,
} from "../types/userAddress";

export function getUserAddresses(): Promise<UserAddress[]> {
  return apiRequest<UserAddress[]>("/api/user-addresses");
}

export function getUserAddressById(id: number): Promise<UserAddress> {
  return apiRequest<UserAddress>(`/api/user-addresses/${id}`);
}

export function createUserAddress(
  request: CreateUserAddressRequest,
): Promise<UserAddress> {
  return apiRequest<UserAddress>("/api/user-addresses", {
    method: "POST",
    body: request,
  });
}

export function updateUserAddress(
  id: number,
  request: UpdateUserAddressRequest,
): Promise<UserAddress> {
  return apiRequest<UserAddress>(`/api/user-addresses/${id}`, {
    method: "PUT",
    body: request,
  });
}

export function deleteUserAddress(id: number): Promise<void> {
  return apiRequest<void>(`/api/user-addresses/${id}`, {
    method: "DELETE",
  });
}

export function setDefaultUserAddress(id: number): Promise<UserAddress> {
  return apiRequest<UserAddress>(`/api/user-addresses/${id}/set-default`, {
    method: "POST",
  });
}