import { apiRequest } from "./client";
import type {
  CreateOrderRequest,
  GetOrdersQuery,
  OrderResponse,
  OrderListItemResponse,
  PagedResult,
} from "../types/orders";

function buildOrdersQueryString(query: GetOrdersQuery = {}): string {
  const params = new URLSearchParams();

  if (query.page !== undefined) {
    params.set("page", String(query.page));
  }

  if (query.pageSize !== undefined) {
    params.set("pageSize", String(query.pageSize));
  }

  if (query.orderStatus !== undefined) {
    params.set("orderStatus", String(query.orderStatus));
  }

  if (query.deliveryStatus !== undefined) {
    params.set("deliveryStatus", String(query.deliveryStatus));
  }

  if (query.paymentStatus !== undefined) {
    params.set("paymentStatus", String(query.paymentStatus));
  }

  if (query.idUser !== undefined) {
    params.set("idUser", String(query.idUser));
  }

  const queryString = params.toString();
  return queryString ? `?${queryString}` : "";
}

export type UpdateOrderStatusesRequest = {
  orderStatus: string;
  deliveryStatus: string;
  paymentStatus: string;
};

export function checkout(request: CreateOrderRequest): Promise<OrderResponse> {
  return apiRequest<OrderResponse>("/api/orders/checkout", {
    method: "POST",
    body: request,
  });
}

export function getMyOrders(
  query: GetOrdersQuery = {},
): Promise<PagedResult<OrderListItemResponse>> {
  return apiRequest<PagedResult<OrderListItemResponse>>(
    `/api/orders${buildOrdersQueryString(query)}`,
  );
}

export function getMyOrderById(id: number): Promise<OrderResponse> {
  return apiRequest<OrderResponse>(`/api/orders/${id}`);
}

export function cancelMyOrder(id: number): Promise<OrderResponse> {
  return apiRequest<OrderResponse>(`/api/orders/${id}/cancel`, {
    method: "POST",
  });
}

export function getAdminOrders(
  query: GetOrdersQuery = {},
): Promise<PagedResult<OrderListItemResponse>> {
  return apiRequest<PagedResult<OrderListItemResponse>>(
    `/api/orders/admin/all${buildOrdersQueryString(query)}`,
  );
}

export function getAdminOrderById(id: number): Promise<OrderResponse> {
  return apiRequest<OrderResponse>(`/api/orders/admin/${id}`);
}

export function updateAdminOrderStatuses(
  id: number,
  request: UpdateOrderStatusesRequest,
): Promise<OrderResponse> {
  return apiRequest<OrderResponse>(`/api/orders/admin/${id}/statuses`, {
    method: "PUT",
    body: request,
  });
}