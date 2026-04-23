import { useEffect, useMemo, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { ApiError } from "../../types/api";
import type { ShoppingCart } from "../../types/cart";
import type {
  CreateOrderRequest,
  OrderDictionaries,
  PaymentMethod,
  PaymentType,
} from "../../types/orders";
import type {
  CreateUserAddressRequest,
  UserAddress,
} from "../../types/userAddress";
import {
  clearCart,
  getCart,
  removeCartItem,
  updateCartItemQuantity,
} from "../../api/cartApi";
import { getOrderDictionaries } from "../../api/orderDictionariesApi";
import { checkout } from "../../api/ordersApi";
import {
  createUserAddress,
  getUserAddresses,
} from "../../api/userAddressesApi";
import "./CartPage.css";

type AddressFormState = {
  recipientName: string;
  phone: string;
  country: string;
  city: string;
  street: string;
  house: string;
  apartment: string;
  postalCode: string;
  comment: string;
  isDefault: boolean;
};

const initialAddressForm: AddressFormState = {
  recipientName: "",
  phone: "",
  country: "",
  city: "",
  street: "",
  house: "",
  apartment: "",
  postalCode: "",
  comment: "",
  isDefault: false,
};

const SITE_SALES_CHANNEL_ID = 1;

function formatPrice(value: number): string {
  return new Intl.NumberFormat("ru-RU", {
    style: "currency",
    currency: "RUB",
    minimumFractionDigits: 2,
  }).format(value);
}

function formatAddress(address: UserAddress): string {
  return [
    address.recipientName,
    address.phone,
    `${address.country}, ${address.city}, ${address.street}, д. ${address.house}`,
    address.apartment ? `кв./офис ${address.apartment}` : null,
    address.postalCode ? `индекс ${address.postalCode}` : null,
  ]
    .filter(Boolean)
    .join(" • ");
}

function mapAddressFormToRequest(
  form: AddressFormState,
): CreateUserAddressRequest {
  return {
    recipientName: form.recipientName.trim(),
    phone: form.phone.trim(),
    country: form.country.trim(),
    city: form.city.trim(),
    street: form.street.trim(),
    house: form.house.trim(),
    apartment: form.apartment.trim() || null,
    postalCode: form.postalCode.trim() || null,
    comment: form.comment.trim() || null,
    isDefault: form.isDefault,
  };
}

export default function CartPage() {
  const navigate = useNavigate();

  const [cart, setCart] = useState<ShoppingCart | null>(null);
  const [addresses, setAddresses] = useState<UserAddress[]>([]);
  const [dictionaries, setDictionaries] = useState<OrderDictionaries | null>(null);

  const [selectedAddressId, setSelectedAddressId] = useState<number | null>(null);
  const [paymentType, setPaymentType] = useState<PaymentType>("Immediate");
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>("Sbp");
  const [comment, setComment] = useState("");

  const [showAddressForm, setShowAddressForm] = useState(false);
  const [addressForm, setAddressForm] = useState<AddressFormState>(initialAddressForm);

  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [updatingItemId, setUpdatingItemId] = useState<number | null>(null);
  const [isClearing, setIsClearing] = useState(false);
  const [isCheckoutLoading, setIsCheckoutLoading] = useState(false);
  const [isCreatingAddress, setIsCreatingAddress] = useState(false);

  const paymentTypeOptions = useMemo(
    () => dictionaries?.paymentTypes ?? [],
    [dictionaries],
  );

  const paymentMethodOptions = useMemo(
    () => dictionaries?.paymentMethods ?? [],
    [dictionaries],
  );

  useEffect(() => {
    void loadPageData();
  }, []);

  async function loadPageData() {
    try {
      setIsLoading(true);
      setError(null);

      const [cartData, addressData, dictionariesData] = await Promise.all([
        getCart(),
        getUserAddresses(),
        getOrderDictionaries(),
      ]);

      setCart(cartData);
      setAddresses(addressData);
      setDictionaries(dictionariesData);

      const defaultAddress =
        addressData.find((x) => x.isDefault) ?? addressData[0] ?? null;

      setSelectedAddressId(defaultAddress?.id ?? null);
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось загрузить данные корзины");
      }
    } finally {
      setIsLoading(false);
    }
  }

  async function loadCartOnly() {
    try {
      setError(null);
      const data = await getCart();
      setCart(data);
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось загрузить корзину");
      }
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
      await loadCartOnly();
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

  async function handleCreateAddress(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    try {
      setIsCreatingAddress(true);
      setError(null);

      const createdAddress = await createUserAddress(
        mapAddressFormToRequest(addressForm),
      );

      setAddresses((prev) => {
        const next = createdAddress.isDefault
          ? prev.map((x) => ({ ...x, isDefault: false }))
          : prev.slice();

        return [createdAddress, ...next];
      });

      setSelectedAddressId(createdAddress.id);
      setAddressForm(initialAddressForm);
      setShowAddressForm(false);
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось создать адрес");
      }
    } finally {
      setIsCreatingAddress(false);
    }
  }

  async function handleCheckout() {
    if (!cart || cart.items.length === 0) {
      setError("Корзина пуста");
      return;
    }

    if (!selectedAddressId) {
      setError("Выберите адрес доставки");
      return;
    }

    try {
      setIsCheckoutLoading(true);
      setError(null);

      const request: CreateOrderRequest = {
        idUserAddress: selectedAddressId,
        idSalesChannel: SITE_SALES_CHANNEL_ID,
        paymentType,
        paymentMethod,
        comment: comment.trim() || null,
        returnUrl: `${window.location.origin}/profile/orders`,
      };

      const order = await checkout(request);

      await loadCartOnly();

      navigate(`/profile/orders/${order.id}`);
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось оформить заказ");
      }
    } finally {
      setIsCheckoutLoading(false);
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

  if (error && !cart) {
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

      {error && <p className="cart-page__error cart-page__error--inline">{error}</p>}

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
                            void handleQuantityChange(item.id, item.quantity - 1)
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
                            void handleQuantityChange(item.id, item.quantity + 1)
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
            <h2 className="cart-summary__title">Оформление заказа</h2>

            <div className="cart-summary__section">
              <div className="cart-summary__section-header">
                <label className="cart-summary__label" htmlFor="address-select">
                  Адрес доставки
                </label>

                <button
                  type="button"
                  className="cart-summary__link-button"
                  onClick={() => setShowAddressForm((prev) => !prev)}
                >
                  {showAddressForm ? "Скрыть форму" : "Новый адрес"}
                </button>
              </div>

              {addresses.length > 0 ? (
                <select
                  id="address-select"
                  className="cart-summary__select"
                  value={selectedAddressId ?? ""}
                  onChange={(e) => setSelectedAddressId(Number(e.target.value))}
                >
                  {addresses.map((address) => (
                    <option key={address.id} value={address.id}>
                      {formatAddress(address)}
                      {address.isDefault ? " (по умолчанию)" : ""}
                    </option>
                  ))}
                </select>
              ) : (
                <p className="cart-summary__hint">
                  У вас пока нет адресов. Добавьте новый адрес ниже.
                </p>
              )}

              {addresses.length > 0 && (
                <p className="cart-summary__hint cart-summary__hint--compact">
                  Управлять всеми адресами можно в{" "}
                  <Link to="/profile/addresses" className="cart-summary__inline-link">
                    профиле
                  </Link>
                  .
                </p>
              )}
            </div>

            {showAddressForm && (
              <form className="cart-address-form" onSubmit={handleCreateAddress}>
                <h3 className="cart-address-form__title">Новый адрес</h3>

                <label className="cart-address-form__field">
                  <span>Получатель</span>
                  <input
                    value={addressForm.recipientName}
                    onChange={(e) =>
                      setAddressForm((prev) => ({
                        ...prev,
                        recipientName: e.target.value,
                      }))
                    }
                    required
                  />
                </label>

                <label className="cart-address-form__field">
                  <span>Телефон</span>
                  <input
                    value={addressForm.phone}
                    onChange={(e) =>
                      setAddressForm((prev) => ({
                        ...prev,
                        phone: e.target.value,
                      }))
                    }
                    required
                  />
                </label>

                <label className="cart-address-form__field">
                  <span>Страна</span>
                  <input
                    value={addressForm.country}
                    onChange={(e) =>
                      setAddressForm((prev) => ({
                        ...prev,
                        country: e.target.value,
                      }))
                    }
                    required
                  />
                </label>

                <label className="cart-address-form__field">
                  <span>Город</span>
                  <input
                    value={addressForm.city}
                    onChange={(e) =>
                      setAddressForm((prev) => ({
                        ...prev,
                        city: e.target.value,
                      }))
                    }
                    required
                  />
                </label>

                <label className="cart-address-form__field">
                  <span>Улица</span>
                  <input
                    value={addressForm.street}
                    onChange={(e) =>
                      setAddressForm((prev) => ({
                        ...prev,
                        street: e.target.value,
                      }))
                    }
                    required
                  />
                </label>

                <label className="cart-address-form__field">
                  <span>Дом</span>
                  <input
                    value={addressForm.house}
                    onChange={(e) =>
                      setAddressForm((prev) => ({
                        ...prev,
                        house: e.target.value,
                      }))
                    }
                    required
                  />
                </label>

                <label className="cart-address-form__field">
                  <span>Квартира / офис</span>
                  <input
                    value={addressForm.apartment}
                    onChange={(e) =>
                      setAddressForm((prev) => ({
                        ...prev,
                        apartment: e.target.value,
                      }))
                    }
                  />
                </label>

                <label className="cart-address-form__field">
                  <span>Почтовый индекс</span>
                  <input
                    value={addressForm.postalCode}
                    onChange={(e) =>
                      setAddressForm((prev) => ({
                        ...prev,
                        postalCode: e.target.value,
                      }))
                    }
                  />
                </label>

                <label className="cart-address-form__field">
                  <span>Комментарий</span>
                  <textarea
                    rows={3}
                    value={addressForm.comment}
                    onChange={(e) =>
                      setAddressForm((prev) => ({
                        ...prev,
                        comment: e.target.value,
                      }))
                    }
                  />
                </label>

                <label className="cart-address-form__checkbox">
                  <input
                    type="checkbox"
                    checked={addressForm.isDefault}
                    onChange={(e) =>
                      setAddressForm((prev) => ({
                        ...prev,
                        isDefault: e.target.checked,
                      }))
                    }
                  />
                  <span>Сделать адресом по умолчанию</span>
                </label>

                <div className="cart-address-form__actions">
                  <button
                    type="submit"
                    className="cart-address-form__submit-button"
                    disabled={isCreatingAddress}
                  >
                    {isCreatingAddress ? "Сохранение..." : "Сохранить адрес"}
                  </button>
                </div>
              </form>
            )}

            <div className="cart-summary__section">
              <label className="cart-summary__label" htmlFor="payment-type">
                Тип оплаты
              </label>
              <select
                id="payment-type"
                className="cart-summary__select"
                value={paymentType}
                onChange={(e) => setPaymentType(e.target.value as PaymentType)}
              >
                {paymentTypeOptions.map((option) => (
                  <option key={option.value} value={option.name}>
                    {option.displayName}
                  </option>
                ))}
              </select>
            </div>

            <div className="cart-summary__section">
              <label className="cart-summary__label" htmlFor="payment-method">
                Способ оплаты
              </label>
              <select
                id="payment-method"
                className="cart-summary__select"
                value={paymentMethod}
                onChange={(e) => setPaymentMethod(e.target.value as PaymentMethod)}
              >
                {paymentMethodOptions.map((option) => (
                  <option key={option.value} value={option.name}>
                    {option.displayName}
                  </option>
                ))}
              </select>
            </div>

            <div className="cart-summary__section">
              <label className="cart-summary__label" htmlFor="order-comment">
                Комментарий к заказу
              </label>
              <textarea
                id="order-comment"
                className="cart-summary__textarea"
                placeholder="Например: позвонить перед доставкой"
                value={comment}
                onChange={(e) => setComment(e.target.value)}
                rows={3}
              />
            </div>

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
              onClick={() => void handleCheckout()}
              disabled={
                isCheckoutLoading || addresses.length === 0 || !selectedAddressId
              }
            >
              {isCheckoutLoading ? "Оформляем..." : "Оформить заказ"}
            </button>
          </aside>
        </div>
      )}
    </section>
  );
}