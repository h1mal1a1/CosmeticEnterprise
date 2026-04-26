export type CartItem = {
  id: number;
  idFinishedProduct: number;
  productName: string;
  mainImageUrl: string | null;
  unitPrice: number;
  quantity: number;
  availableQuantity: number;
  hasEnoughStock: boolean;
  lineTotal: number;
};

export type ShoppingCart = {
  id: number;
  idUser: number;
  items: CartItem[];
  totalItemsQuantity: number;
  totalAmount: number;
};

export type AddCartItemRequest = {
  idFinishedProduct: number;
  quantity: number;
};

export type UpdateCartItemQuantityRequest = {
  quantity: number;
};