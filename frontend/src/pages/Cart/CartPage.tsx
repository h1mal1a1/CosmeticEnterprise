import { useEffect, useState } from "react";
import { ApiError } from "../../types/api";
import type { ShoppingCart } from "../../types/cart";
import {
    clearCart,
    getCart,
    removeCartItem,
    updateCartItemQuantity,
} from "../../api/cartApi";
import "./CartPage.css";

function formatPrice(value: number): string {
    return new Intl.NumberFormat("ru-RU", {
        style: "currency",
        currency: "RUB",
        minimumFractionDigits: 2,
    }).format(value);
}

export default function CartPage() {
    const [cart, setCart] = useState<ShoppingCart | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [updatingItemId, setUpdatingItemId] = useState<number | null>(null);
    const [isClearing, setIsClearing] = useState(false);

    useEffect(() => {
        void loadCart();
    }, []);

    async function loadCart() {
        try {
            setIsLoading(true);
            setError(null);
            const data = await getCart();
            setCart(data);
        } catch (err) {
            if (err instanceof ApiError) {
                setError(err.message);
            } else {
                setError("Не удалось загрузить корзину");
            }
        } finally {
            setIsLoading(false);
        }
    }

    async function handleQuantityChange(itemId: number, quantity: number) {
        if (quantity < 1) {
            return;
        }

        try {
            setUpdatingItemId(itemId);
            setError(null);
            const updatedCart = await updateCartItemQuantity(itemId, { quantity });
            setCart(updatedCart);
        } catch (err) {
            if (err instanceof ApiError) {
                setError(err.message);
            } else {
                setError("Не удалось изменить количество товара");
            }
        } finally {
            setUpdatingItemId(null);
        }
    }

    async function handleRemoveItem(itemId: number) {
        try {
            setUpdatingItemId(itemId);
            setError(null);
            const updatedCart = await removeCartItem(itemId);
            setCart(updatedCart);
        } catch (err) {
            if (err instanceof ApiError) {
                setError(err.message);
            } else {
                setError("Не удалось удалить товар из корзины");
            }
        } finally {
            setUpdatingItemId(null);
        }
    }

    async function handleClearCart() {
        try {
            setIsClearing(true);
            setError(null);
            await clearCart();
            await loadCart();
        } catch (err) {
            if (err instanceof ApiError) {
                setError(err.message);
            } else {
                setError("Не удалось очистить корзину");
            }
        } finally {
            setIsClearing(false);
        }
    }

    if (isLoading) {
        return (
            <section className="cart-page">
                <h1 className="cart-page__title">Корзина</h1>
                <p className="cart-page__status">Загрузка корзины...</p>
            </section>
        );
    }

    if (error) {
        return (
            <section className="cart-page">
                <h1 className="cart-page__title">Корзина</h1>
                <p className="cart-page__error">{error}</p>
            </section>
        );
    }

    const items = cart?.items ?? [];
    const isEmpty = items.length === 0;

    return (
        <section className="cart-page">
            <div className="cart-page__header">
                <div>
                    <h1 className="cart-page__title">Корзина</h1>
                    <p className="cart-page__subtitle">
                        {isEmpty
                            ? "Ваша корзина пока пуста"
                            : `Товаров в корзине: ${cart?.totalItemsQuantity ?? 0}`}
                    </p>
                </div>

                {!isEmpty && (
                    <button
                        type="button"
                        className="cart-page__clear-button"
                        onClick={() => void handleClearCart()}
                        disabled={isClearing}
                    >
                        {isClearing ? "Очистка..." : "Очистить корзину"}
                    </button>
                )}
            </div>

            {isEmpty ? (
                <div className="cart-page__empty">
                    <p>Добавьте товары, чтобы оформить заказ.</p>
                </div>
            ) : (
                <div className="cart-page__layout">
                    <div className="cart-list">
                        {items.map((item) => {
                            const isUpdating = updatingItemId === item.id;

                            return (
                                <article key={item.id} className="cart-item">
                                    <div className="cart-item__image-wrapper">
                                        {item.mainImageUrl ? (
                                            <img
                                                src={item.mainImageUrl}
                                                alt={item.productName}
                                                className="cart-item__image"
                                            />
                                        ) : (
                                            <div className="cart-item__image cart-item__image--placeholder">
                                                Нет фото
                                            </div>
                                        )}
                                    </div>

                                    <div className="cart-item__content">
                                        <h2 className="cart-item__title">{item.productName}</h2>
                                        <p className="cart-item__price">
                                            Цена за единицу: {formatPrice(item.unitPrice)}
                                        </p>

                                        <div className="cart-item__controls">
                                            <div className="cart-item__quantity">
                                                <button
                                                    type="button"
                                                    className="cart-item__quantity-button"
                                                    onClick={() =>
                                                        void handleQuantityChange(
                                                            item.id,
                                                            item.quantity - 1,
                                                        )
                                                    }
                                                    disabled={isUpdating || item.quantity <= 1}
                                                >
                                                    −
                                                </button>

                                                <span className="cart-item__quantity-value">
                                                    {item.quantity}
                                                </span>

                                                <button
                                                    type="button"
                                                    className="cart-item__quantity-button"
                                                    onClick={() =>
                                                        void handleQuantityChange(
                                                            item.id,
                                                            item.quantity + 1,
                                                        )
                                                    }
                                                    disabled={isUpdating || item.quantity >= 999}
                                                >
                                                    +
                                                </button>
                                            </div>

                                            <button
                                                type="button"
                                                className="cart-item__remove-button"
                                                onClick={() => void handleRemoveItem(item.id)}
                                                disabled={isUpdating}
                                            >
                                                Удалить
                                            </button>
                                        </div>
                                    </div>

                                    <div className="cart-item__total">
                                        {formatPrice(item.lineTotal)}
                                    </div>
                                </article>
                            );
                        })}
                    </div>

                    <aside className="cart-summary">
                        <h2 className="cart-summary__title">Итого</h2>

                        <div className="cart-summary__row">
                            <span>Количество товаров</span>
                            <strong>{cart?.totalItemsQuantity ?? 0}</strong>
                        </div>

                        <div className="cart-summary__row cart-summary__row--total">
                            <span>Сумма</span>
                            <strong>{formatPrice(cart?.totalAmount ?? 0)}</strong>
                        </div>

                        <button
                            type="button"
                            className="cart-summary__checkout-button"
                            disabled
                        >
                            Оформление заказа
                        </button>
                    </aside>
                </div>
            )}
        </section>
    );
}