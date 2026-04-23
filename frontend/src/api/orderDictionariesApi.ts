import { apiRequest } from "./client";
import type { OrderDictionaries } from "../types/orders";

export function getOrderDictionaries(): Promise<OrderDictionaries> {
  return apiRequest<OrderDictionaries>("/api/order-dictionaries");
}