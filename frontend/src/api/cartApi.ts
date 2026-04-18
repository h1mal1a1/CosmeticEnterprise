import { apiRequest } from "./client";
import type {
    AddCartItemRequest,
    ShoppingCart,
    UpdateCartItemQuantityRequest,
} from "../types/cart";

export function getCart(): Promise<ShoppingCart> {
    return apiRequest<ShoppingCart>("/api/cart");
}

export function addCartItem(
    request: AddCartItemRequest,
): Promise<ShoppingCart> {
    return apiRequest<ShoppingCart>("/api/cart/items", {
        method: "POST",
        body: request,
    });
}

export function updateCartItemQuantity(
    itemId: number,
    request: UpdateCartItemQuantityRequest,
): Promise<ShoppingCart> {
    return apiRequest<ShoppingCart>(`/api/cart/items/${itemId}`, {
        method: "PUT",
        body: request,
    });
}

export function removeCartItem(itemId: number): Promise<ShoppingCart> {
    return apiRequest<ShoppingCart>(`/api/cart/items/${itemId}`, {
        method: "DELETE",
    });
}

export function clearCart(): Promise<void> {
    return apiRequest<void>("/api/cart", {
        method: "DELETE",
    });
}