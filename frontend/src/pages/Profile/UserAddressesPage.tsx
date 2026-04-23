import { useEffect, useMemo, useState } from "react";
import { ApiError } from "../../types/api";
import type {
  CreateUserAddressRequest,
  UpdateUserAddressRequest,
  UserAddress,
} from "../../types/userAddress";
import {
  createUserAddress,
  deleteUserAddress,
  getUserAddresses,
  setDefaultUserAddress,
  updateUserAddress,
} from "../../api/userAddressesApi";
import "./UserAddressesPage.css";

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

const initialFormState: AddressFormState = {
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

function mapAddressToForm(address: UserAddress): AddressFormState {
  return {
    recipientName: address.recipientName,
    phone: address.phone,
    country: address.country,
    city: address.city,
    street: address.street,
    house: address.house,
    apartment: address.apartment ?? "",
    postalCode: address.postalCode ?? "",
    comment: address.comment ?? "",
    isDefault: address.isDefault,
  };
}

function mapFormToCreateRequest(
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

function mapFormToUpdateRequest(
  form: AddressFormState,
): UpdateUserAddressRequest {
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

export default function UserAddressesPage() {
  const [addresses, setAddresses] = useState<UserAddress[]>([]);
  const [form, setForm] = useState<AddressFormState>(initialFormState);
  const [editingAddressId, setEditingAddressId] = useState<number | null>(null);

  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [busyAddressId, setBusyAddressId] = useState<number | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    void loadAddresses();
  }, []);

  async function loadAddresses() {
    try {
      setIsLoading(true);
      setError(null);
      const data = await getUserAddresses();
      setAddresses(data);
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось загрузить адреса");
      }
    } finally {
      setIsLoading(false);
    }
  }

  function resetForm() {
    setForm(initialFormState);
    setEditingAddressId(null);
  }

  function handleEdit(address: UserAddress) {
    setForm(mapAddressToForm(address));
    setEditingAddressId(address.id);
    setError(null);
  }

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    try {
      setIsSaving(true);
      setError(null);

      if (editingAddressId === null) {
        const created = await createUserAddress(mapFormToCreateRequest(form));
        setAddresses((prev) => {
          const next = form.isDefault
            ? prev.map((x) => ({ ...x, isDefault: false }))
            : prev.slice();

          return [created, ...next].sort(sortAddresses);
        });
      } else {
        const updated = await updateUserAddress(
          editingAddressId,
          mapFormToUpdateRequest(form),
        );

        setAddresses((prev) => {
          let next = prev.map((x) =>
            x.id === updated.id ? updated : x,
          );

          if (updated.isDefault) {
            next = next.map((x) =>
              x.id === updated.id ? x : { ...x, isDefault: false },
            );
          }

          return next.sort(sortAddresses);
        });
      }

      resetForm();
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось сохранить адрес");
      }
    } finally {
      setIsSaving(false);
    }
  }

  async function handleDelete(addressId: number) {
    const confirmed = window.confirm("Удалить этот адрес?");
    if (!confirmed) {
      return;
    }

    try {
      setBusyAddressId(addressId);
      setError(null);
      await deleteUserAddress(addressId);
      setAddresses((prev) => prev.filter((x) => x.id !== addressId));

      if (editingAddressId === addressId) {
        resetForm();
      }

      await loadAddresses();
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось удалить адрес");
      }
    } finally {
      setBusyAddressId(null);
    }
  }

  async function handleSetDefault(addressId: number) {
    try {
      setBusyAddressId(addressId);
      setError(null);
      const updated = await setDefaultUserAddress(addressId);

      setAddresses((prev) =>
        prev
          .map((x) =>
            x.id === updated.id
              ? updated
              : { ...x, isDefault: false },
          )
          .sort(sortAddresses),
      );

      if (editingAddressId === updated.id) {
        setForm((prev) => ({ ...prev, isDefault: true }));
      }
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("Не удалось сделать адрес основным");
      }
    } finally {
      setBusyAddressId(null);
    }
  }

  const pageTitle = useMemo(
    () => (editingAddressId === null ? "Новый адрес" : "Редактирование адреса"),
    [editingAddressId],
  );

  if (isLoading) {
    return (
      <section className="user-addresses-page">
        <h1 className="user-addresses-page__title">Мои адреса</h1>
        <p className="user-addresses-page__status">Загрузка адресов...</p>
      </section>
    );
  }

  return (
    <section className="user-addresses-page">
      <div className="user-addresses-page__header">
        <div>
          <h1 className="user-addresses-page__title">Мои адреса</h1>
          <p className="user-addresses-page__subtitle">
            Управляйте адресами доставки для оформления заказов
          </p>
        </div>

        {editingAddressId !== null && (
          <button
            type="button"
            className="user-addresses-page__secondary-button"
            onClick={resetForm}
          >
            Новый адрес
          </button>
        )}
      </div>

      {error && <p className="user-addresses-page__error">{error}</p>}

      <div className="user-addresses-page__layout">
        <div className="user-addresses-list">
          {addresses.length === 0 ? (
            <div className="user-address-card user-address-card--empty">
              У вас пока нет адресов
            </div>
          ) : (
            addresses.map((address) => {
              const isBusy = busyAddressId === address.id;

              return (
                <article key={address.id} className="user-address-card">
                  <div className="user-address-card__top">
                    <div>
                      <h2 className="user-address-card__title">
                        {address.recipientName}
                      </h2>
                      <p className="user-address-card__phone">{address.phone}</p>
                    </div>

                    {address.isDefault && (
                      <span className="user-address-card__badge">
                        По умолчанию
                      </span>
                    )}
                  </div>

                  <div className="user-address-card__body">
                    <p>
                      {address.country}, {address.city}, {address.street}, д.{" "}
                      {address.house}
                      {address.apartment ? `, ${address.apartment}` : ""}
                    </p>

                    {address.postalCode && (
                      <p>Индекс: {address.postalCode}</p>
                    )}

                    {address.comment && <p>Комментарий: {address.comment}</p>}
                  </div>

                  <div className="user-address-card__actions">
                    {!address.isDefault && (
                      <button
                        type="button"
                        className="user-address-card__action-button"
                        onClick={() => void handleSetDefault(address.id)}
                        disabled={isBusy}
                      >
                        Сделать основным
                      </button>
                    )}

                    <button
                      type="button"
                      className="user-address-card__action-button"
                      onClick={() => handleEdit(address)}
                      disabled={isBusy}
                    >
                      Изменить
                    </button>

                    <button
                      type="button"
                      className="user-address-card__action-button user-address-card__action-button--danger"
                      onClick={() => void handleDelete(address.id)}
                      disabled={isBusy}
                    >
                      Удалить
                    </button>
                  </div>
                </article>
              );
            })
          )}
        </div>

        <aside className="user-address-form-card">
          <h2 className="user-address-form-card__title">{pageTitle}</h2>

          <form className="user-address-form" onSubmit={handleSubmit}>
            <label className="user-address-form__field">
              <span>Получатель</span>
              <input
                value={form.recipientName}
                onChange={(e) =>
                  setForm((prev) => ({ ...prev, recipientName: e.target.value }))
                }
                required
              />
            </label>

            <label className="user-address-form__field">
              <span>Телефон</span>
              <input
                value={form.phone}
                onChange={(e) =>
                  setForm((prev) => ({ ...prev, phone: e.target.value }))
                }
                required
              />
            </label>

            <label className="user-address-form__field">
              <span>Страна</span>
              <input
                value={form.country}
                onChange={(e) =>
                  setForm((prev) => ({ ...prev, country: e.target.value }))
                }
                required
              />
            </label>

            <label className="user-address-form__field">
              <span>Город</span>
              <input
                value={form.city}
                onChange={(e) =>
                  setForm((prev) => ({ ...prev, city: e.target.value }))
                }
                required
              />
            </label>

            <label className="user-address-form__field">
              <span>Улица</span>
              <input
                value={form.street}
                onChange={(e) =>
                  setForm((prev) => ({ ...prev, street: e.target.value }))
                }
                required
              />
            </label>

            <label className="user-address-form__field">
              <span>Дом</span>
              <input
                value={form.house}
                onChange={(e) =>
                  setForm((prev) => ({ ...prev, house: e.target.value }))
                }
                required
              />
            </label>

            <label className="user-address-form__field">
              <span>Квартира / офис</span>
              <input
                value={form.apartment}
                onChange={(e) =>
                  setForm((prev) => ({ ...prev, apartment: e.target.value }))
                }
              />
            </label>

            <label className="user-address-form__field">
              <span>Почтовый индекс</span>
              <input
                value={form.postalCode}
                onChange={(e) =>
                  setForm((prev) => ({ ...prev, postalCode: e.target.value }))
                }
              />
            </label>

            <label className="user-address-form__field">
              <span>Комментарий</span>
              <textarea
                value={form.comment}
                onChange={(e) =>
                  setForm((prev) => ({ ...prev, comment: e.target.value }))
                }
                rows={3}
              />
            </label>

            <label className="user-address-form__checkbox">
              <input
                type="checkbox"
                checked={form.isDefault}
                onChange={(e) =>
                  setForm((prev) => ({ ...prev, isDefault: e.target.checked }))
                }
              />
              <span>Сделать адресом по умолчанию</span>
            </label>

            <div className="user-address-form__actions">
              <button
                type="submit"
                className="user-address-form__submit-button"
                disabled={isSaving}
              >
                {isSaving ? "Сохранение..." : "Сохранить"}
              </button>

              {editingAddressId !== null && (
                <button
                  type="button"
                  className="user-address-form__cancel-button"
                  onClick={resetForm}
                  disabled={isSaving}
                >
                  Отмена
                </button>
              )}
            </div>
          </form>
        </aside>
      </div>
    </section>
  );
}

function sortAddresses(a: UserAddress, b: UserAddress): number {
  if (a.isDefault && !b.isDefault) return -1;
  if (!a.isDefault && b.isDefault) return 1;
  return new Date(b.updatedAtUtc).getTime() - new Date(a.updatedAtUtc).getTime();
}