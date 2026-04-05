import { apiRequest } from "./client";

export type UnitOfMeasurement = {
  id: number;
  name: string;
};

export type CreateUnitOfMeasurementRequest = {
  name: string;
};

export type UpdateUnitOfMeasurementRequest = {
  name: string;
};

export function getUnitsOfMeasurement(): Promise<UnitOfMeasurement[]> {
  return apiRequest<UnitOfMeasurement[]>("/api/units-of-measurement");
}

export function createUnitOfMeasurement(
  request: CreateUnitOfMeasurementRequest,
): Promise<UnitOfMeasurement> {
  return apiRequest<UnitOfMeasurement>("/api/units-of-measurement", {
    method: "POST",
    body: request,
  });
}

export function updateUnitOfMeasurement(
  id: number,
  request: UpdateUnitOfMeasurementRequest,
): Promise<void> {
  return apiRequest<void>(`/api/units-of-measurement/${id}`, {
    method: "PUT",
    body: request,
  });
}

export function deleteUnitOfMeasurement(id: number): Promise<void> {
  return apiRequest<void>(`/api/units-of-measurement/${id}`, {
    method: "DELETE",
  });
}