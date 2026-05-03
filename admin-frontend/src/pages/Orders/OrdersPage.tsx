import { useEffect, useMemo, useState } from "react";
import { Button, useNotify, useRedirect } from "react-admin";
import { getOrderDictionaries } from "../../api/orderDictionariesApi";
import { getAdminOrders } from "../../api/ordersApi";
import type {
  EnumOption,
  OrderDictionaries,
  OrderListItemResponse,
} from "../../types/orders";
import {
  getDeliveryStatusLabel,
  getOrderStatusLabel,
  getPaymentStatusLabel,
} from "../../utils/orderPresentation";
import "./OrdersPage.css";

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

export default function OrdersPage() {
  const redirect = useRedirect();
  const notify = useNotify();

  const [orders, setOrders] = useState<OrderListItemResponse[]>([]);
  const [dictionaries, setDictionaries] =
    useState<OrderDictionaries | null>(null);

  const [orderStatusFilter, setOrderStatusFilter] = useState("");
  const [paymentStatusFilter, setPaymentStatusFilter] = useState("");
  const [deliveryStatusFilter, setDeliveryStatusFilter] = useState("");
  const [userIdFilter, setUserIdFilter] = useState("");

  const [page, setPage] = useState(1);
  const pageSize = 12;
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);

  const [isLoading, setIsLoading] = useState(true);

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
    } catch (error) {
      console.error(error);
      notify("Не удалось загрузить справочники заказов", { type: "warning" });
    }
  }

  async function loadOrders() {
    try {
      setIsLoading(true);

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
    } catch (error) {
      console.error(error);
      notify("Не удалось загрузить заказы", { type: "error" });
    } finally {
      setIsLoading(false);
    }
  }

  async function handleApplyFilters() {
    setPage(1);
    await loadOrders();
  }

  return (
    <section className="orders-page">
      <header className="orders-page__header">
        <div>
          <h1>Заказы</h1>
          <p>Управление заказами и фильтрами</p>
        </div>
      </header>

      <div className="orders-page__filters">
        <label className="orders-page__filter">
          <span>Статус заказа</span>
          <select
            value={orderStatusFilter}
            onChange={(event) => setOrderStatusFilter(event.target.value)}
          >
            <option value="">Все</option>
            {orderStatusOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.displayName}
              </option>
            ))}
          </select>
        </label>

        <label className="orders-page__filter">
          <span>Статус оплаты</span>
          <select
            value={paymentStatusFilter}
            onChange={(event) => setPaymentStatusFilter(event.target.value)}
          >
            <option value="">Все</option>
            {paymentStatusOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.displayName}
              </option>
            ))}
          </select>
        </label>

        <label className="orders-page__filter">
          <span>Статус доставки</span>
          <select
            value={deliveryStatusFilter}
            onChange={(event) => setDeliveryStatusFilter(event.target.value)}
          >
            <option value="">Все</option>
            {deliveryStatusOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.displayName}
              </option>
            ))}
          </select>
        </label>

        <label className="orders-page__filter">
          <span>ID пользователя</span>
          <input
            type="number"
            value={userIdFilter}
            onChange={(event) => setUserIdFilter(event.target.value)}
            placeholder="Например: 5"
          />
        </label>

        <button
          type="button"
          className="orders-page__apply-button"
          onClick={() => void handleApplyFilters()}
        >
          Применить
        </button>
      </div>

      <div className="orders-page__card">
        <div className="orders-page__card-top">
          <h2>Список заказов</h2>
          <span>Всего: {totalCount}</span>
        </div>

        {isLoading ? (
          <p className="orders-page__state">Загрузка...</p>
        ) : orders.length === 0 ? (
          <p className="orders-page__state">Заказы не найдены</p>
        ) : (
          <div className="orders-page__list">
            {orders.map((order) => (
              <article key={order.id} className="orders-page__order-card">
                <div className="orders-page__order-top">
                  <strong>Заказ №{order.id}</strong>
                  <span>{formatPrice(order.totalAmount)}</span>
                </div>

                <div className="orders-page__order-meta">
                  <span>
                    Пользователь:{" "}
                    {order.username?.trim()
                      ? order.username
                      : `id ${order.idUser}`}
                  </span>

                  <span>
                    Статус заказа:{" "}
                    {dictionaries
                      ? getDisplayName(orderStatusOptions, order.orderStatus)
                      : getOrderStatusLabel(order.orderStatus)}
                  </span>

                  <span>
                    Оплата:{" "}
                    {dictionaries
                      ? getDisplayName(paymentStatusOptions, order.paymentStatus)
                      : getPaymentStatusLabel(order.paymentStatus)}
                  </span>

                  <span>
                    Доставка:{" "}
                    {dictionaries
                      ? getDisplayName(
                          deliveryStatusOptions,
                          order.deliveryStatus,
                        )
                      : getDeliveryStatusLabel(order.deliveryStatus)}
                  </span>
                </div>

                <div className="orders-page__order-bottom">
                  <span>{formatDate(order.createdAtUtc)}</span>

                  <Button
                    label="Открыть"
                    onClick={() => redirect(`/orders/${order.id}`)}
                  />
                </div>
              </article>
            ))}
          </div>
        )}

        {totalPages > 1 && (
          <div className="orders-page__pagination">
            <button
              type="button"
              onClick={() => setPage((current) => current - 1)}
              disabled={page <= 1}
            >
              Назад
            </button>

            <span>
              {page} / {totalPages}
            </span>

            <button
              type="button"
              onClick={() => setPage((current) => current + 1)}
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