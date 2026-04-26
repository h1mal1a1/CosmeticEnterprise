import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { ApiError } from "../../types/api";
import type {
  EnumOption,
  OrderDictionaries,
  OrderListItemResponse,
} from "../../types/orders";
import { getOrderDictionaries } from "../../api/orderDictionariesApi";
import { getAdminOrders } from "../../api/ordersApi";
import "../../styles/status-badges.css";
import {
  getDeliveryStatusBadgeClass,
  getDeliveryStatusLabel,
  getOrderStatusBadgeClass,
  getOrderStatusLabel,
  getPaymentStatusBadgeClass,
  getPaymentStatusLabel,
} from "../../utils/orderPresentation";
import "./AdminOrdersPage.css";

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

export default function AdminOrdersPage() {
  const [orders, setOrders] = useState<OrderListItemResponse[]>([]);
  const [dictionaries, setDictionaries] =
    useState<OrderDictionaries | null>(null);

  const [orderStatusFilter, setOrderStatusFilter] = useState<string>("");
  const [paymentStatusFilter, setPaymentStatusFilter] = useState<string>("");
  const [deliveryStatusFilter, setDeliveryStatusFilter] = useState<string>("");
  const [userIdFilter, setUserIdFilter] = useState("");

  const [page, setPage] = useState(1);
  const pageSize = 12;
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);

  const [isLoading, setIsLoading] = useState(true);
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

  useEffect(() => {
    void loadDictionaries();
  }, []);

  useEffect(() => {
    void loadOrders();
  }, [page, orderStatusFilter, paymentStatusFilter, deliveryStatusFilter]);

  async function loadDictionaries() {
    try {
      const data = await getOrderDictionaries();
      setDictionaries(data);
    } catch {
      // ignore
    }
  }

  async function loadOrders() {
    try {
      setIsLoading(true);
      setError(null);

      const result = await getAdminOrders({
        page,
        pageSize,
        orderStatus: orderStatusFilter ? Number(orderStatusFilter) : undefined,
        paymentStatus: paymentStatusFilter
          ? Number(paymentStatusFilter)
          : undefined,
        deliveryStatus: deliveryStatusFilter
          ? Number(deliveryStatusFilter)
          : undefined,
        idUser: userIdFilter.trim() ? Number(userIdFilter) : undefined,
      });

      setOrders(result.items);
      setTotalPages(result.totalPages || 1);
      setTotalCount(result.totalCount);
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

  async function handleApplyFilters() {
    setPage(1);
    await loadOrders();
  }

  return (
    <section className="admin-orders-page">
      <header className="admin-orders-page__header">
        <div>
          <h1>Заказы</h1>
          <p>Управление заказами и фильтрами</p>
        </div>
      </header>

      {error && <p className="admin-orders-page__error">{error}</p>}

      <div className="admin-orders-page__filters">
        <label className="admin-orders-page__filter">
          <span>Статус заказа</span>
          <select
            value={orderStatusFilter}
            onChange={(e) => setOrderStatusFilter(e.target.value)}
          >
            <option value="">Все</option>
            {orderStatusOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.displayName}
              </option>
            ))}
          </select>
        </label>

        <label className="admin-orders-page__filter">
          <span>Статус оплаты</span>
          <select
            value={paymentStatusFilter}
            onChange={(e) => setPaymentStatusFilter(e.target.value)}
          >
            <option value="">Все</option>
            {paymentStatusOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.displayName}
              </option>
            ))}
          </select>
        </label>

        <label className="admin-orders-page__filter">
          <span>Статус доставки</span>
          <select
            value={deliveryStatusFilter}
            onChange={(e) => setDeliveryStatusFilter(e.target.value)}
          >
            <option value="">Все</option>
            {deliveryStatusOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.displayName}
              </option>
            ))}
          </select>
        </label>

        <label className="admin-orders-page__filter">
          <span>Id пользователя</span>
          <input
            type="number"
            value={userIdFilter}
            onChange={(e) => setUserIdFilter(e.target.value)}
            placeholder="Например: 5"
          />
        </label>

        <button
          type="button"
          className="admin-orders-page__apply-button"
          onClick={() => void handleApplyFilters()}
        >
          Применить
        </button>
      </div>

      <div className="admin-orders-list-card">
        <div className="admin-orders-list-card__top">
          <h2>Список заказов</h2>
          <span>Всего: {totalCount}</span>
        </div>

        {isLoading ? (
          <p className="admin-orders-list-card__status">Загрузка...</p>
        ) : orders.length === 0 ? (
          <p className="admin-orders-list-card__status">Заказы не найдены</p>
        ) : (
          <div className="admin-orders-list">
            {orders.map((order) => (
              <article key={order.id} className="admin-order-card">
                <div className="admin-order-card__top">
                  <strong>Заказ №{order.id}</strong>
                  <span>{formatPrice(order.totalAmount)}</span>
                </div>

                <div className="admin-order-card__meta">
                  <span>Пользователь: {order.idUser}</span>

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

                <div className="admin-order-card__bottom">
                  <span className="admin-order-card__date">
                    {formatDate(order.createdAtUtc)}
                  </span>

                  <Link
                    to={`/admin/orders/${order.id}`}
                    className="admin-order-card__open-button"
                  >
                    Открыть
                  </Link>
                </div>
              </article>
            ))}
          </div>
        )}

        {totalPages > 1 && (
          <div className="admin-orders-list-card__pagination">
            <button
              type="button"
              onClick={() => setPage((prev) => prev - 1)}
              disabled={page <= 1}
            >
              Назад
            </button>
            <span>
              {page} / {totalPages}
            </span>
            <button
              type="button"
              onClick={() => setPage((prev) => prev + 1)}
              disabled={page >= totalPages}
            >
              Вперед
            </button>
          </div>
        )}
      </div>
    </section>
  );
}