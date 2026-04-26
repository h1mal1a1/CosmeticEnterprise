import { useEffect, useMemo, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { ApiError } from "../../types/api";
import type {
  DeliveryStatus,
  EnumOption,
  OrderDictionaries,
  OrderResponse,
  OrderStatus,
  PaymentStatus,
} from "../../types/orders";
import { getOrderDictionaries } from "../../api/orderDictionariesApi";
import {
  getAdminOrderById,
  updateAdminOrderStatuses,
} from "../../api/ordersApi";
import "../../styles/status-badges.css";
import {
  getDeliveryStatusBadgeClass,
  getDeliveryStatusLabel,
  getOrderStatusBadgeClass,
  getOrderStatusLabel,
  getPaymentStatusBadgeClass,
  getPaymentStatusLabel,
} from "../../utils/orderPresentation";
import "./AdminOrderDetailsPage.css";

type FormState = {
  orderStatus: OrderStatus;
  deliveryStatus: DeliveryStatus;
  paymentStatus: PaymentStatus;
};

function formatPrice(value: number): string {
  return new Intl.NumberFormat("ru-RU", {
    style: "currency",
    currency: "RUB",
    minimumFractionDigits: 2,
  }).format(value);
}

function formatDate(value: string): string {
  return new Intl.DateTimeFormat("ru-RU", {
    dateStyle: "medium",
    timeStyle: "short",
  }).format(new Date(value));
}

function getDisplayName(
  options: EnumOption[],
  value: string | number,
): string {
  return (
    options.find((x) => x.value === value || x.name === value)?.displayName ??
    String(value)
  );
}

export default function AdminOrderDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [order, setOrder] = useState<OrderResponse | null>(null);
  const [dictionaries, setDictionaries] = useState<OrderDictionaries | null>(null);
  const [form, setForm] = useState<FormState | null>(null);

  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const orderStatusOptions = useMemo(
    () => dictionaries?.orderStatuses ?? [],
    [dictionaries],
  );

  const deliveryStatusOptions = useMemo(
    () => dictionaries?.deliveryStatuses ?? [],
    [dictionaries],
  );

  const paymentStatusOptions = useMemo(
    () => dictionaries?.paymentStatuses ?? [],
    [dictionaries],
  );

  const paymentTypeOptions = useMemo(
    () => dictionaries?.paymentTypes ?? [],
    [dictionaries],
  );

  const paymentMethodOptions = useMemo(
    () => dictionaries?.paymentMethods ?? [],
    [dictionaries],
  );

  useEffect(() => {
    void loadPage();
  }, [id]);

  async function loadPage() {
    if (!id) {
      setError("Не указан идентификатор заказа");
      setIsLoading(false);
      return;
    }

    try {
      setIsLoading(true);
      setError(null);

      const orderId = Number(id);

      const [orderData, dictionariesData] = await Promise.all([
        getAdminOrderById(orderId),
        getOrderDictionaries(),
      ]);

      setOrder(orderData);
      setDictionaries(dictionariesData);
      setForm({
        orderStatus: orderData.orderStatus,
        deliveryStatus: orderData.deliveryStatus,
        paymentStatus: orderData.paymentStatus,
      });
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось загрузить заказ");
      }
    } finally {
      setIsLoading(false);
    }
  }

  async function handleSaveStatuses() {
    if (!id || !form) {
      return;
    }

    try {
      setIsSaving(true);
      setError(null);

      const updated = await updateAdminOrderStatuses(Number(id), {
        orderStatus: form.orderStatus,
        deliveryStatus: form.deliveryStatus,
        paymentStatus: form.paymentStatus,
      });

      setOrder(updated);
      setForm({
        orderStatus: updated.orderStatus,
        deliveryStatus: updated.deliveryStatus,
        paymentStatus: updated.paymentStatus,
      });
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось сохранить статусы");
      }
    } finally {
      setIsSaving(false);
    }
  }

  if (isLoading) {
    return (
      <section className="admin-order-details-page">
        <h1 className="admin-order-details-page__title">Заказ</h1>
        <p className="admin-order-details-page__status">Загрузка заказа...</p>
      </section>
    );
  }

  if (error || !order || !form) {
    return (
      <section className="admin-order-details-page">
        <div className="admin-order-details-page__topbar">
          <button
            type="button"
            className="admin-order-details-page__back-button"
            onClick={() => navigate("/admin/orders")}
          >
            ← К заказам
          </button>
        </div>

        <p className="admin-order-details-page__error">
          {error ?? "Заказ не найден"}
        </p>
      </section>
    );
  }

  return (
    <section className="admin-order-details-page">
      <div className="admin-order-details-page__topbar">
        <Link
          to="/admin/orders"
          className="admin-order-details-page__back-button"
        >
          ← К заказам
        </Link>
      </div>

      <div className="admin-order-details-page__header">
        <div>
          <h1 className="admin-order-details-page__title">
            Заказ №{order.id}
          </h1>
          <p className="admin-order-details-page__subtitle">
            Создан: {formatDate(order.createdAtUtc)}
          </p>
        </div>

        <button
          type="button"
          className="admin-order-details-page__save-button"
          onClick={() => void handleSaveStatuses()}
          disabled={isSaving}
        >
          {isSaving ? "Сохранение..." : "Сохранить статусы"}
        </button>
      </div>

      {error && <p className="admin-order-details-page__error">{error}</p>}

      <div className="admin-order-details-page__layout">
        <div className="admin-order-details-page__main">
          <div className="admin-order-details-card">
            <h2 className="admin-order-details-card__title">Позиции заказа</h2>

            <div className="admin-order-items-list">
              {order.items.map((item) => (
                <article key={item.id} className="admin-order-item-card">
                  <div className="admin-order-item-card__content">
                    <h3 className="admin-order-item-card__title">
                      {item.productName}
                    </h3>

                    <div className="admin-order-item-card__meta">
                      <span>Количество: {item.quantity}</span>
                      <span>Цена: {formatPrice(item.unitPrice)}</span>
                    </div>
                  </div>

                  <div className="admin-order-item-card__total">
                    {formatPrice(item.lineTotal)}
                  </div>
                </article>
              ))}
            </div>
          </div>
        </div>

        <aside className="admin-order-details-page__sidebar">
          <div className="admin-order-details-card">
            <h2 className="admin-order-details-card__title">
              Информация о заказе
            </h2>

            <div className="admin-order-details-info">
              <div className="admin-order-details-info__row">
                <span>Пользователь</span>
                <strong>{order.idUser}</strong>
              </div>

              <div className="admin-order-details-info__row">
                <span>Адрес</span>
                <strong>{order.idUserAddress}</strong>
              </div>

              <div className="admin-order-details-info__row">
                <span>Статус заказа</span>
                <span className={getOrderStatusBadgeClass(order.orderStatus)}>
                  {dictionaries
                    ? getDisplayName(orderStatusOptions, order.orderStatus)
                    : getOrderStatusLabel(order.orderStatus)}
                </span>
              </div>

              <div className="admin-order-details-info__row">
                <span>Статус доставки</span>
                <span className={getDeliveryStatusBadgeClass(order.deliveryStatus)}>
                  {getDeliveryStatusLabel(order.deliveryStatus)}
                </span>
              </div>

              <div className="admin-order-details-info__row">
                <span>Статус оплаты</span>
                <span className={getPaymentStatusBadgeClass(order.paymentStatus)}>
                  {getPaymentStatusLabel(order.paymentStatus)}
                </span>
              </div>

              <div className="admin-order-details-info__row">
                <span>Тип оплаты</span>
                <strong>
                  {dictionaries
                    ? getDisplayName(paymentTypeOptions, order.paymentType)
                    : order.paymentType}
                </strong>
              </div>

              <div className="admin-order-details-info__row">
                <span>Способ оплаты</span>
                <strong>
                  {dictionaries
                    ? getDisplayName(paymentMethodOptions, order.paymentMethod)
                    : order.paymentMethod}
                </strong>
              </div>

              <div className="admin-order-details-info__row">
                <span>Обновлен</span>
                <strong>{formatDate(order.updatedAtUtc)}</strong>
              </div>

              {order.comment && (
                <div className="admin-order-details-info__comment">
                  <span>Комментарий</span>
                  <p>{order.comment}</p>
                </div>
              )}
            </div>

            <div className="admin-order-status-form">
              <label className="admin-order-status-form__field">
                <span>Статус заказа</span>
                <select
                  value={form.orderStatus}
                  onChange={(e) =>
                    setForm((prev) =>
                      prev
                        ? {
                            ...prev,
                            orderStatus: e.target.value as OrderStatus,
                          }
                        : prev,
                    )
                  }
                >
                  {orderStatusOptions.map((option) => (
                    <option key={option.value} value={option.name}>
                      {option.displayName}
                    </option>
                  ))}
                </select>
              </label>

              <label className="admin-order-status-form__field">
                <span>Статус доставки</span>
                <select
                  value={form.deliveryStatus}
                  onChange={(e) =>
                    setForm((prev) =>
                      prev
                        ? {
                            ...prev,
                            deliveryStatus: e.target.value as DeliveryStatus,
                          }
                        : prev,
                    )
                  }
                >
                  {deliveryStatusOptions.map((option) => (
                    <option key={option.value} value={option.name}>
                      {option.displayName}
                    </option>
                  ))}
                </select>
              </label>

              <label className="admin-order-status-form__field">
                <span>Статус оплаты</span>
                <select
                  value={form.paymentStatus}
                  onChange={(e) =>
                    setForm((prev) =>
                      prev
                        ? {
                            ...prev,
                            paymentStatus: e.target.value as PaymentStatus,
                          }
                        : prev,
                    )
                  }
                >
                  {paymentStatusOptions.map((option) => (
                    <option key={option.value} value={option.name}>
                      {option.displayName}
                    </option>
                  ))}
                </select>
              </label>
            </div>

            <div className="admin-order-details-total">
              <div className="admin-order-details-total__row">
                <span>Доставка</span>
                <strong>{formatPrice(order.deliveryPrice)}</strong>
              </div>

              <div className="admin-order-details-total__row admin-order-details-total__row--final">
                <span>Итого</span>
                <strong>{formatPrice(order.totalAmount)}</strong>
              </div>
            </div>
          </div>
        </aside>
      </div>
    </section>
  );
}