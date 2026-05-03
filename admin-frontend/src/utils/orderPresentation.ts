import type {
  DeliveryStatus,
  PaymentMethod,
  PaymentStatus,
  PaymentType,
  OrderStatus,
} from "../types/orders";

export function getOrderStatusLabel(status: OrderStatus): string {
  const labels: Record<OrderStatus, string> = {
    Created: "Создан",
    AwaitingPayment: "Ожидает оплаты",
    Paid: "Оплачен",
    Processing: "В обработке",
    Completed: "Завершен",
    Cancelled: "Отменен",
  };

  return labels[status] ?? status;
}

export function getDeliveryStatusLabel(status: DeliveryStatus): string {
  const labels: Record<DeliveryStatus, string> = {
    Pending: "Ожидает",
    Preparing: "Готовится",
    Shipped: "Отправлен",
    Delivered: "Доставлен",
    Cancelled: "Отменен",
  };

  return labels[status] ?? status;
}

export function getPaymentStatusLabel(status: PaymentStatus): string {
  const labels: Record<PaymentStatus, string> = {
    Pending: "Ожидает",
    Paid: "Оплачено",
    Failed: "Ошибка оплаты",
  };

  return labels[status] ?? status;
}

export function getPaymentTypeLabel(type: PaymentType): string {
  const labels: Record<PaymentType, string> = {
    1: "Сразу",
    2: "Постоплата",
  };

  return labels[type] ?? String(type);
}

export function getPaymentMethodLabel(method: PaymentMethod): string {
  const labels: Record<PaymentMethod, string> = {
    1: "Наличные",
    2: "Банковский перевод",
    3: "СБП",
  };

  return labels[method] ?? String(method);
}