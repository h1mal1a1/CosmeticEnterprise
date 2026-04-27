import { useEffect, useState } from "react";
import "./ProfilePage.css";
import { Link, useNavigate } from "react-router-dom";
import { updateProfile } from "../../api/authApi";
import { useAuth } from "../../components/auth/AuthProvider";

type FormState = {
  email: string;
  phone: string;
};

export default function ProfilePage() {
  const { user, logout, refreshUser } = useAuth();
  const navigate = useNavigate();

  const [isEditing, setIsEditing] = useState(false);
  const [form, setForm] = useState<FormState>({
    email: "",
    phone: "",
  });
  const [error, setError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    setForm({
      email: user?.email ?? "",
      phone: user?.phone ?? "",
    });
  }, [user]);

  async function handleLogout() {
    await logout();
    navigate("/", { replace: true });
  }

  function handleEdit() {
    setError("");
    setSuccessMessage("");
    setIsEditing(true);
  }

  function handleCancel() {
    setForm({
      email: user?.email ?? "",
      phone: user?.phone ?? "",
    });
    setError("");
    setSuccessMessage("");
    setIsEditing(false);
  }

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const email = form.email.trim();
    const phone = form.phone.trim();

    if (!email) {
      setError("Укажите почту.");
      return;
    }

    if (!phone) {
      setError("Укажите телефон.");
      return;
    }

    if (!/^\+[1-9]\d{7,14}$/.test(phone)) {
      setError("Телефон должен быть в формате +79261234567.");
      return;
    }

    try {
      setIsSaving(true);
      setError("");
      setSuccessMessage("");

      await updateProfile({
        email,
        phone,
      });

      await refreshUser();

      setIsEditing(false);
      setSuccessMessage("Данные профиля обновлены.");
    } catch {
      setError("Не удалось обновить данные. Возможно, почта или телефон уже используются.");
    } finally {
      setIsSaving(false);
    }
  }

  return (
    <div className="profile-page">
      <div className="profile-card">
        <div className="profile-card__header">
          <div>
            <h1>Личный кабинет</h1>
            <p>Информация о текущем пользователе</p>
          </div>

          {!isEditing && (
            <button
              type="button"
              className="profile-edit-button"
              onClick={handleEdit}
            >
              Изменить
            </button>
          )}
        </div>

        {error && <div className="profile-alert profile-alert--error">{error}</div>}
        {successMessage && (
          <div className="profile-alert profile-alert--success">
            {successMessage}
          </div>
        )}

        {!isEditing ? (
          <div className="profile-card__content">
            <div className="profile-field">
              <span className="profile-field__label">Имя пользователя</span>
              <span className="profile-field__value">
                {user?.username ?? "Неизвестно"}
              </span>
            </div>

            <div className="profile-field">
              <span className="profile-field__label">Почта</span>
              <span className="profile-field__value">
                {user?.email?.trim() ? user.email : "Не указана"}
              </span>
            </div>

            <div className="profile-field">
              <span className="profile-field__label">Телефон</span>
              <span className="profile-field__value">
                {user?.phone?.trim() ? user.phone : "Не указан"}
              </span>
            </div>
          </div>
        ) : (
          <form className="profile-form" onSubmit={handleSubmit}>
            <div className="profile-field">
              <span className="profile-field__label">Имя пользователя</span>
              <span className="profile-field__value">
                {user?.username ?? "Неизвестно"}
              </span>
            </div>

            <label className="profile-form__field">
              <span className="profile-field__label">Почта</span>
              <input
                type="email"
                value={form.email}
                onChange={(event) =>
                  setForm((prev) => ({
                    ...prev,
                    email: event.target.value,
                  }))
                }
                disabled={isSaving}
                required
              />
            </label>

            <label className="profile-form__field">
              <span className="profile-field__label">Телефон</span>
              <input
                type="tel"
                value={form.phone}
                onChange={(event) =>
                  setForm((prev) => ({
                    ...prev,
                    phone: event.target.value,
                  }))
                }
                disabled={isSaving}
                placeholder="+79261234567"
                required
              />
            </label>

            <div className="profile-form__actions">
              <button
                type="submit"
                className="profile-save-button"
                disabled={isSaving}
              >
                {isSaving ? "Сохранение..." : "Сохранить"}
              </button>

              <button
                type="button"
                className="profile-cancel-button"
                onClick={handleCancel}
                disabled={isSaving}
              >
                Отмена
              </button>
            </div>
          </form>
        )}

        <div className="profile-actions">
          <Link to="/profile/addresses" className="profile-actions__link">
            Мои адреса
          </Link>

          <Link to="/profile/orders" className="profile-actions__link">
            Мои заказы
          </Link>

          <Link to="/cart" className="profile-actions__link">
            Перейти в корзину
          </Link>
        </div>

        <button className="profile-logout-button" onClick={handleLogout}>
          Выйти
        </button>
      </div>
    </div>
  );
}