import { useEffect, useMemo, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { ApiError } from "../../types/api";
import type {
  EnumOption,
  OrderDictionaries,
  OrderResponse,
} from "../../types/orders";
import { getOrderDictionaries } from "../../api/orderDictionariesApi";
import { cancelMyOrder, getMyOrderById } from "../../api/ordersApi";
import "../../styles/status-badges.css";
import {
  getDeliveryStatusBadgeClass,
  getDeliveryStatusLabel,
  getOrderStatusBadgeClass,
  getOrderStatusLabel,
  getPaymentStatusBadgeClass,
  getPaymentStatusLabel,
} from "../../utils/orderPresentation";
import "./OrderDetailsPage.css";

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

export default function OrderDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [order, setOrder] = useState<OrderResponse | null>(null);
  const [dictionaries, setDictionaries] = useState<OrderDictionaries | null>(null);

  const [isLoading, setIsLoading] = useState(true);
  const [isCancelling, setIsCancelling] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const orderStatusOptions = useMemo(
    () => dictionaries?.orderStatuses ?? [],
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
        getMyOrderById(orderId),
        getOrderDictionaries(),
      ]);

      setOrder(orderData);
      setDictionaries(dictionariesData);
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

  async function handleCancelOrder() {
    if (!order) {
      return;
    }

    const confirmed = window.confirm("Отменить этот заказ?");
    if (!confirmed) {
      return;
    }

    try {
      setIsCancelling(true);
      setError(null);
      const updated = await cancelMyOrder(order.id);
      setOrder(updated);
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось отменить заказ");
      }
    } finally {
      setIsCancelling(false);
    }
  }

  if (isLoading) {
    return (
      <section className="order-details-page">
        <h1 className="order-details-page__title">Заказ</h1>
        <p className="order-details-page__status">Загрузка заказа...</p>
      </section>
    );
  }

  if (error || !order) {
    return (
      <section className="order-details-page">
        <div className="order-details-page__topbar">
          <button
            type="button"
            className="order-details-page__back-button"
            onClick={() => navigate("/profile/orders")}
          >
            ← К заказам
          </button>
        </div>

        <p className="order-details-page__error">
          {error ?? "Заказ не найден"}
        </p>
      </section>
    );
  }

  const canCancel =
    order.orderStatus !== "Cancelled" && order.orderStatus !== "Completed";

  return (
    <section className="order-details-page">
      <div className="order-details-page__topbar">
        <Link to="/profile/orders" className="order-details-page__back-button">
          ← К заказам
        </Link>
      </div>

      <div className="order-details-page__header">
        <div>
          <h1 className="order-details-page__title">Заказ №{order.id}</h1>
          <p className="order-details-page__subtitle">
            Создан: {formatDate(order.createdAtUtc)}
          </p>
        </div>

        {canCancel && (
          <button
            type="button"
            className="order-details-page__cancel-button"
            onClick={() => void handleCancelOrder()}
            disabled={isCancelling}
          >
            {isCancelling ? "Отмена..." : "Отменить заказ"}
          </button>
        )}
      </div>

      {error && <p className="order-details-page__error">{error}</p>}

      <div className="order-details-page__layout">
        <div className="order-details-page__main">
          <div className="order-details-card">
            <h2 className="order-details-card__title">Состав заказа</h2>

            <div className="order-items-list">
              {order.items.map((item) => (
                <article key={item.id} className="order-item-card">
                  <div className="order-item-card__content">
                    <h3 className="order-item-card__title">{item.productName}</h3>

                    <div className="order-item-card__meta">
                      <span>Количество: {item.quantity}</span>
                      <span>Цена: {formatPrice(item.unitPrice)}</span>
                    </div>
                  </div>

                  <div className="order-item-card__total">
                    {formatPrice(item.lineTotal)}
                  </div>
                </article>
              ))}
            </div>
          </div>
        </div>

        <aside className="order-details-page__sidebar">
          <div className="order-details-card">
            <h2 className="order-details-card__title">Информация о заказе</h2>

            <div className="order-details-info">
              <div className="order-details-info__row">
                <span>Статус заказа</span>
                <span className={getOrderStatusBadgeClass(order.orderStatus)}>
                  {dictionaries
                    ? getDisplayName(orderStatusOptions, order.orderStatus)
                    : getOrderStatusLabel(order.orderStatus)}
                </span>
              </div>

              <div className="order-details-info__row">
                <span>Статус доставки</span>
                <span className={getDeliveryStatusBadgeClass(order.deliveryStatus)}>
                  {getDeliveryStatusLabel(order.deliveryStatus)}
                </span>
              </div>

              <div className="order-details-info__row">
                <span>Статус оплаты</span>
                <span className={getPaymentStatusBadgeClass(order.paymentStatus)}>
                  {getPaymentStatusLabel(order.paymentStatus)}
                </span>
              </div>

              <div className="order-details-info__row">
                <span>Тип оплаты</span>
                <strong>
                  {dictionaries
                    ? getDisplayName(paymentTypeOptions, order.paymentType)
                    : order.paymentType}
                </strong>
              </div>

              <div className="order-details-info__row">
                <span>Способ оплаты</span>
                <strong>
                  {dictionaries
                    ? getDisplayName(paymentMethodOptions, order.paymentMethod)
                    : order.paymentMethod}
                </strong>
              </div>

              <div className="order-details-info__row">
                <span>Дата обновления</span>
                <strong>{formatDate(order.updatedAtUtc)}</strong>
              </div>

              {order.comment && (
                <div className="order-details-info__comment">
                  <span>Комментарий</span>
                  <p>{order.comment}</p>
                </div>
              )}
            </div>

            <div className="order-details-total">
              <div className="order-details-total__row">
                <span>Доставка</span>
                <strong>{formatPrice(order.deliveryPrice)}</strong>
              </div>

              <div className="order-details-total__row order-details-total__row--final">
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