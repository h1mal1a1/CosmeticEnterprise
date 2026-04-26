export type OrderStatus =
  | "Created"
  | "AwaitingPayment"
  | "Paid"
  | "Processing"
  | "Completed"
  | "Cancelled";

export type DeliveryStatus =
  | "Pending"
  | "Preparing"
  | "Shipped"
  | "Delivered"
  | "Cancelled";

export type PaymentType = 1 | 2;
// 1 = Immediate, 2 = Postpaid

export type PaymentMethod = 1 | 2 | 3;
// 1 = Cash, 2 = BankTransfer, 3 = Sbp

export type PaymentStatus = "Pending" | "Paid" | "Failed";

export type EnumOption = {
  value: number;
  name: string;
  displayName: string;
};

export type OrderDictionaries = {
  orderStatuses: EnumOption[];
  deliveryStatuses: EnumOption[];
  paymentTypes: EnumOption[];
  paymentMethods: EnumOption[];
  paymentStatuses: EnumOption[];
};

export type CreateOrderRequest = {
  idUserAddress: number;
  paymentType: PaymentType;
  paymentMethod: PaymentMethod;
  comment?: string | null;
  returnUrl?: string | null;
};

export type OrderItemResponse = {
  id: number;
  idFinishedProduct: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
};

export type OrderResponse = {
  id: number;
  idUser: number;
  idUserAddress: number;
  idSalesChannel: number;
  orderStatus: OrderStatus;
  deliveryStatus: DeliveryStatus;
  paymentType: PaymentType;
  paymentMethod: PaymentMethod;
  paymentStatus: PaymentStatus;
  totalAmount: number;
  deliveryPrice: number;
  comment?: string | null;
  createdAtUtc: string;
  updatedAtUtc: string;
  items: OrderItemResponse[];
};

export type OrderListItemResponse = {
  id: number;
  idUser: number;
  idUserAddress: number;
  idSalesChannel: number;
  orderStatus: OrderStatus;
  deliveryStatus: DeliveryStatus;
  paymentType: PaymentType;
  paymentMethod: PaymentMethod;
  paymentStatus: PaymentStatus;
  totalAmount: number;
  deliveryPrice: number;
  totalItemsQuantity: number;
  createdAtUtc: string;
  updatedAtUtc: string;
};

export type GetOrdersQuery = {
  page?: number;
  pageSize?: number;
  orderStatus?: number;
  deliveryStatus?: number;
  paymentStatus?: number;
  idUser?: number;
};

export type PagedResult<T> = {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};