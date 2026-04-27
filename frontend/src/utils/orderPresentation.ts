import type {
  DeliveryStatus,
  OrderStatus,
  PaymentMethod,
  PaymentStatus,
  PaymentType,
} from "../types/orders";

export function getOrderStatusLabel(status: OrderStatus): string {
  switch (status) {
    case "Created":
      return "Создан";
    case "AwaitingPayment":
      return "Ожидает оплаты";
    case "Paid":
      return "Оплачен";
    case "Processing":
      return "В обработке";
    case "Completed":
      return "Завершен";
    case "Cancelled":
      return "Отменен";
    default:
      return status;
  }
}

export function getDeliveryStatusLabel(status: DeliveryStatus): string {
  switch (status) {
    case "Pending":
      return "Ожидает обработки";
    case "Preparing":
      return "Собирается";
    case "Shipped":
      return "Отправлен";
    case "Delivered":
      return "Доставлен";
    case "Cancelled":
      return "Отменен";
    default:
      return status;
  }
}

export function getPaymentStatusLabel(status: PaymentStatus): string {
  switch (status) {
    case "Pending":
      return "Ожидает оплаты";
    case "Paid":
      return "Оплачен";
    case "Failed":
      return "Ошибка оплаты";
    default:
      return status;
  }
}

export function getPaymentTypeLabel(paymentType: PaymentType): string {
  switch (paymentType) {
    case 1:
      return "Оплата сразу";
    case 2:
      return "Постоплата";
    default:
      return String(paymentType);
  }
}

export function getPaymentMethodLabel(paymentMethod: PaymentMethod): string {
  switch (paymentMethod) {
    case 1:
      return "Наличными";
    case 2:
      return "Перевод";
    case 3:
      return "СБП";
    default:
      return String(paymentMethod);
  }
}

export function getOrderStatusBadgeClass(status: OrderStatus): string {
  switch (status) {
    case "Completed":
      return "status-badge status-badge--success";
    case "Cancelled":
      return "status-badge status-badge--danger";
    case "AwaitingPayment":
      return "status-badge status-badge--warning";
    case "Processing":
      return "status-badge status-badge--info";
    case "Paid":
      return "status-badge status-badge--success";
    case "Created":
    default:
      return "status-badge status-badge--neutral";
  }
}

export function getPaymentStatusBadgeClass(status: PaymentStatus): string {
  switch (status) {
    case "Paid":
      return "status-badge status-badge--success";
    case "Failed":
      return "status-badge status-badge--danger";
    case "Pending":
    default:
      return "status-badge status-badge--warning";
  }
}

export function getDeliveryStatusBadgeClass(status: DeliveryStatus): string {
  switch (status) {
    case "Delivered":
      return "status-badge status-badge--success";
    case "Cancelled":
      return "status-badge status-badge--danger";
    case "Shipped":
      return "status-badge status-badge--info";
    case "Preparing":
      return "status-badge status-badge--warning";
    case "Pending":
    default:
      return "status-badge status-badge--neutral";
  }
}