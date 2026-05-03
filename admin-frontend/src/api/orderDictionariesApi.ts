import type { OrderDictionaries } from "../types/orders";

const API_URL = "/api";

export async function getOrderDictionaries(): Promise<OrderDictionaries> {
  const response = await fetch(`${API_URL}/order-dictionaries`, {
    credentials: "include",
  });

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`);
  }

  return response.json();
}