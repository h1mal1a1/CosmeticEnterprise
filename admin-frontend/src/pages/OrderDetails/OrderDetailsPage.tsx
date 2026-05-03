import { useEffect, useMemo, useState } from "react";
import { Button, useNotify, useRedirect } from "react-admin";
import { useParams } from "react-router-dom";
import { getOrderDictionaries } from "../../api/orderDictionariesApi";
import {
  getAdminOrderById,
  updateAdminOrderStatuses,
} from "../../api/ordersApi";
import type {
  DeliveryStatus,
  EnumOption,
  OrderDictionaries,
  OrderResponse,
  OrderStatus,
  PaymentStatus,
} from "../../types/orders";
import {
  getPaymentMethodLabel,
  getPaymentTypeLabel,
} from "../../utils/orderPresentation";
import "./OrderDetailsPage.css";

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

function getDisplayName(options: EnumOption[], value: string | number): string {
  return (
    options.find((x) => x.value === value || x.name === value)?.displayName ??
    String(value)
  );
}

function getEnumValue(options: EnumOption[], name: string): number {
  const option = options.find((x) => x.name === name);

  if (!option) {
    throw new Error(`Enum option '${name}' not found.`);
  }

  return option.value;
}

function isFormChanged(order: OrderResponse, form: FormState): boolean {
  return (
    order.orderStatus !== form.orderStatus ||
    order.deliveryStatus !== form.deliveryStatus ||
    order.paymentStatus !== form.paymentStatus
  );
}

export default function OrderDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const redirect = useRedirect();
  const notify = useNotify();

  const [order, setOrder] = useState<OrderResponse | null>(null);
  const [dictionaries, setDictionaries] =
    useState<OrderDictionaries | null>(null);
  const [form, setForm] = useState<FormState | null>(null);

  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);

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
      notify("Не указан идентификатор заказа", { type: "error" });
      setIsLoading(false);
      return;
    }

    try {
      setIsLoading(true);

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
    } catch (error) {
      console.error(error);
      notify("Не удалось загрузить заказ", { type: "error" });
    } finally {
      setIsLoading(false);
    }
  }

  async function handleSaveStatuses() {
    if (!id || !form || !dictionaries) {
      return;
    }

    try {
      setIsSaving(true);

      const updated = await updateAdminOrderStatuses(Number(id), {
        orderStatus: getEnumValue(orderStatusOptions, form.orderStatus),
        deliveryStatus: getEnumValue(deliveryStatusOptions, form.deliveryStatus),
        paymentStatus: getEnumValue(paymentStatusOptions, form.paymentStatus),
      });

      setOrder(updated);
      setForm({
        orderStatus: updated.orderStatus,
        deliveryStatus: updated.deliveryStatus,
        paymentStatus: updated.paymentStatus,
      });

      notify("Статусы заказа сохранены", { type: "success" });
    } catch (error) {
      console.error(error);
      notify("Не удалось сохранить статусы", { type: "error" });
    } finally {
      setIsSaving(false);
    }
  }

  if (isLoading) {
    return (
      <section className="order-details-page">
        <p className="order-details-page__state">Загрузка заказа...</p>
      </section>
    );
  }

  if (!order || !form) {
    return (
      <section className="order-details-page">
        <Button label="Назад к заказам" onClick={() => redirect("/orders")} />
        <p className="order-details-page__state">Заказ не найден</p>
      </section>
    );
  }

  const hasChanges = isFormChanged(order, form);

  return (
    <section className="order-details-page">
      <div className="order-details-page__topbar">
        <Button label="Назад к заказам" onClick={() => redirect("/orders")} />
      </div>

      <div className="order-details-page__header">
        <div>
          <h1>Заказ №{order.id}</h1>
          <p>Создан: {formatDate(order.createdAtUtc)}</p>
        </div>

        <button
          type="button"
          className="order-details-page__save-button"
          onClick={() => void handleSaveStatuses()}
          disabled={isSaving || !hasChanges || !dictionaries}
        >
          {isSaving ? "Сохранение..." : "Сохранить статусы"}
        </button>
      </div>

      <div className="order-details-page__layout">
        <div className="order-details-page__main">
          <div className="order-details-page__card">
            <h2>Позиции заказа</h2>

            <div className="order-details-page__items">
              {order.items.map((item) => (
                <article key={item.id} className="order-details-page__item">
                  <div>
                    <h3>{item.productName}</h3>
                    <p>
                      Количество: {item.quantity} · Цена:{" "}
                      {formatPrice(item.unitPrice)}
                    </p>
                  </div>

                  <strong>{formatPrice(item.lineTotal)}</strong>
                </article>
              ))}
            </div>
          </div>
        </div>

        <aside className="order-details-page__sidebar">
          <div className="order-details-page__card">
            <h2>Информация о заказе</h2>

            <div className="order-details-page__info">
              <div className="order-details-page__row">
                <span>Пользователь</span>
                <strong>
                  {order.username?.trim()
                    ? order.username
                    : `id ${order.idUser}`}
                </strong>
              </div>

              <div className="order-details-page__row">
                <span>Адрес</span>
                <strong>{order.deliveryAddress}</strong>
              </div>

              <div className="order-details-page__editor">
                <h3>Изменить статусы</h3>

                <label>
                  <span>Статус заказа</span>
                  <select
                    value={form.orderStatus}
                    onChange={(event) =>
                      setForm((current) =>
                        current
                          ? {
                              ...current,
                              orderStatus: event.target.value as OrderStatus,
                            }
                          : current,
                      )
                    }
                  >
                    {orderStatusOptions.map((option) => (
                      <option key={option.name} value={option.name}>
                        {option.displayName}
                      </option>
                    ))}
                  </select>
                </label>

                <label>
                  <span>Статус доставки</span>
                  <select
                    value={form.deliveryStatus}
                    onChange={(event) =>
                      setForm((current) =>
                        current
                          ? {
                              ...current,
                              deliveryStatus: event.target
                                .value as DeliveryStatus,
                            }
                          : current,
                      )
                    }
                  >
                    {deliveryStatusOptions.map((option) => (
                      <option key={option.name} value={option.name}>
                        {option.displayName}
                      </option>
                    ))}
                  </select>
                </label>

                <label>
                  <span>Статус оплаты</span>
                  <select
                    value={form.paymentStatus}
                    onChange={(event) =>
                      setForm((current) =>
                        current
                          ? {
                              ...current,
                              paymentStatus: event.target
                                .value as PaymentStatus,
                            }
                          : current,
                      )
                    }
                  >
                    {paymentStatusOptions.map((option) => (
                      <option key={option.name} value={option.name}>
                        {option.displayName}
                      </option>
                    ))}
                  </select>
                </label>
              </div>

              <div className="order-details-page__row">
                <span>Тип оплаты</span>
                <strong>
                  {dictionaries
                    ? getDisplayName(paymentTypeOptions, order.paymentType)
                    : getPaymentTypeLabel(order.paymentType)}
                </strong>
              </div>

              <div className="order-details-page__row">
                <span>Способ оплаты</span>
                <strong>
                  {dictionaries
                    ? getDisplayName(paymentMethodOptions, order.paymentMethod)
                    : getPaymentMethodLabel(order.paymentMethod)}
                </strong>
              </div>

              <div className="order-details-page__row">
                <span>Обновлен</span>
                <strong>{formatDate(order.updatedAtUtc)}</strong>
              </div>

              {order.comment && (
                <div className="order-details-page__comment">
                  <span>Комментарий</span>
                  <p>{order.comment}</p>
                </div>
              )}
            </div>

            <div className="order-details-page__total">
              <div className="order-details-page__row">
                <span>Доставка</span>
                <strong>{formatPrice(order.deliveryPrice)}</strong>
              </div>

              <div className="order-details-page__row order-details-page__row--final">
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