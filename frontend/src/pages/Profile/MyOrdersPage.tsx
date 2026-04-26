import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { ApiError } from "../../types/api";
import type {
  EnumOption,
  OrderDictionaries,
  OrderListItemResponse,
} from "../../types/orders";
import { getOrderDictionaries } from "../../api/orderDictionariesApi";
import { cancelMyOrder, getMyOrders } from "../../api/ordersApi";
import "../../styles/status-badges.css";
import {
  getDeliveryStatusBadgeClass,
  getDeliveryStatusLabel,
  getOrderStatusBadgeClass,
  getOrderStatusLabel,
  getPaymentStatusBadgeClass,
  getPaymentStatusLabel,
} from "../../utils/orderPresentation";
import "./MyOrdersPage.css";

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

function getDisplayName(options: EnumOption[], value: string | number): string {
  return (
    options.find((x) => x.value === value || x.name === value)?.displayName ??
    String(value)
  );
}

export default function MyOrdersPage() {
  const [orders, setOrders] = useState<OrderListItemResponse[]>([]);
  const [dictionaries, setDictionaries] =
    useState<OrderDictionaries | null>(null);

  const [selectedOrderStatus, setSelectedOrderStatus] = useState<string>("");
  const [page, setPage] = useState(1);
  const pageSize = 10;

  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(1);

  const [isLoading, setIsLoading] = useState(true);
  const [busyOrderId, setBusyOrderId] = useState<number | null>(null);
  const [error, setError] = useState<string | null>(null);

  const orderStatusOptions = useMemo(
    () => dictionaries?.orderStatuses ?? [],
    [dictionaries],
  );

  const deliveryStatusOptions = useMemo(
    () => dictionaries?.deliveryStatuses ?? [],
    [dictionaries],
  );

  const paymentMethodOptions = useMemo(
    () => dictionaries?.paymentMethods ?? [],
    [dictionaries],
  );

  const paymentStatusOptions = useMemo(
    () => dictionaries?.paymentStatuses ?? [],
    [dictionaries],
  );

  useEffect(() => {
    void loadOrdersPage();
  }, [page, selectedOrderStatus]);

  useEffect(() => {
    void loadDictionaries();
  }, []);

  async function loadDictionaries() {
    try {
      const data = await getOrderDictionaries();
      setDictionaries(data);
    } catch {
      // ignore
    }
  }

  async function loadOrdersPage() {
    try {
      setIsLoading(true);
      setError(null);

      const result = await getMyOrders({
        page,
        pageSize,
        orderStatus: selectedOrderStatus
          ? Number(selectedOrderStatus)
          : undefined,
      });

      setOrders(result.items);
      setTotalCount(result.totalCount);
      setTotalPages(result.totalPages || 1);
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось загрузить заказы");
      }
    } finally {
      setIsLoading(false);
    }
  }

  async function handleCancelOrder(orderId: number) {
    const confirmed = window.confirm("Отменить этот заказ?");
    if (!confirmed) {
      return;
    }

    try {
      setBusyOrderId(orderId);
      setError(null);
      await cancelMyOrder(orderId);
      await loadOrdersPage();
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось отменить заказ");
      }
    } finally {
      setBusyOrderId(null);
    }
  }

  function handleStatusFilterChange(value: string) {
    setSelectedOrderStatus(value);
    setPage(1);
  }

  if (isLoading) {
    return (
      <section className="my-orders-page">
        <h1 className="my-orders-page__title">Мои заказы</h1>
        <p className="my-orders-page__status">Загрузка заказов...</p>
      </section>
    );
  }

  return (
    <section className="my-orders-page">
      <div className="my-orders-page__header">
        <div>
          <h1 className="my-orders-page__title">Мои заказы</h1>
          <p className="my-orders-page__subtitle">
            Всего заказов: {totalCount}
          </p>
        </div>

        <div className="my-orders-page__actions">
          <Link to="/profile" className="my-orders-page__link-button">
            В личный кабинет
          </Link>
        </div>
      </div>

      {error && <p className="my-orders-page__error">{error}</p>}

      <div className="my-orders-page__filters">
        <label className="my-orders-page__filter">
          <span>Статус заказа</span>
          <select
            value={selectedOrderStatus}
            onChange={(e) => handleStatusFilterChange(e.target.value)}
          >
            <option value="">Все</option>
            {orderStatusOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.displayName}
              </option>
            ))}
          </select>
        </label>
      </div>

      {orders.length === 0 ? (
        <div className="my-orders-page__empty">У вас пока нет заказов</div>
      ) : (
        <div className="my-orders-list">
          {orders.map((order) => {
            const canCancel =
              order.orderStatus !== "Cancelled" &&
              order.orderStatus !== "Completed";

            const isBusy = busyOrderId === order.id;

            return (
              <article key={order.id} className="my-order-card">
                <div className="my-order-card__top">
                  <div>
                    <h2 className="my-order-card__title">Заказ №{order.id}</h2>
                    <p className="my-order-card__date">
                      Создан: {formatDate(order.createdAtUtc)}
                    </p>
                  </div>

                  <div className="my-order-card__badges">
                    <span className={getOrderStatusBadgeClass(order.orderStatus)}>
                      {dictionaries
                        ? getDisplayName(orderStatusOptions, order.orderStatus)
                        : getOrderStatusLabel(order.orderStatus)}
                    </span>

                    <span className={getPaymentStatusBadgeClass(order.paymentStatus)}>
                      {dictionaries
                        ? getDisplayName(paymentStatusOptions, order.paymentStatus)
                        : getPaymentStatusLabel(order.paymentStatus)}
                    </span>

                    <span className={getDeliveryStatusBadgeClass(order.deliveryStatus)}>
                      {dictionaries
                        ? getDisplayName(deliveryStatusOptions, order.deliveryStatus)
                        : getDeliveryStatusLabel(order.deliveryStatus)}
                    </span>
                  </div>
                </div>

                <div className="my-order-card__grid">
                  <div className="my-order-card__field">
                    <span className="my-order-card__label">Оплата</span>
                    <span className="my-order-card__value">
                      {dictionaries
                        ? getDisplayName(paymentMethodOptions, order.paymentMethod)
                        : order.paymentMethod}
                    </span>
                  </div>

                  <div className="my-order-card__field">
                    <span className="my-order-card__label">Статус оплаты</span>
                    <span className="my-order-card__value">
                      {dictionaries
                        ? getDisplayName(paymentStatusOptions, order.paymentStatus)
                        : getPaymentStatusLabel(order.paymentStatus)}
                    </span>
                  </div>

                  <div className="my-order-card__field">
                    <span className="my-order-card__label">Доставка</span>
                    <span className="my-order-card__value">
                      {dictionaries
                        ? getDisplayName(deliveryStatusOptions, order.deliveryStatus)
                        : getDeliveryStatusLabel(order.deliveryStatus)}
                    </span>
                  </div>

                  <div className="my-order-card__field">
                    <span className="my-order-card__label">Товаров</span>
                    <span className="my-order-card__value">
                      {order.totalItemsQuantity}
                    </span>
                  </div>
                </div>

                <div className="my-order-card__bottom">
                  <div className="my-order-card__amount">
                    {formatPrice(order.totalAmount)}
                  </div>

                  <div className="my-order-card__actions">
                    <Link
                      to={`/profile/orders/${order.id}`}
                      className="my-order-card__action-button"
                    >
                      Подробнее
                    </Link>

                    {canCancel && (
                      <button
                        type="button"
                        className="my-order-card__action-button my-order-card__action-button--danger"
                        onClick={() => void handleCancelOrder(order.id)}
                        disabled={isBusy}
                      >
                        {isBusy ? "Отмена..." : "Отменить"}
                      </button>
                    )}
                  </div>
                </div>
              </article>
            );
          })}
        </div>
      )}

      {totalPages > 1 && (
        <div className="my-orders-page__pagination">
          <button
            type="button"
            className="my-orders-page__pagination-button"
            onClick={() => setPage((prev) => prev - 1)}
            disabled={page <= 1}
          >
            Назад
          </button>

          <span className="my-orders-page__pagination-info">
            Страница {page} из {totalPages}
          </span>

          <button
            type="button"
            className="my-orders-page__pagination-button"
            onClick={() => setPage((prev) => prev + 1)}
            disabled={page >= totalPages}
          >
            Вперед
          </button>
        </div>
      )}
    </section>
  );
}