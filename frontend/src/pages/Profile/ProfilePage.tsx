import "./ProfilePage.css";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../components/auth/AuthProvider";

export default function ProfilePage() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  async function handleLogout() {
    await logout();
    navigate("/", { replace: true });
  }

  return (
    <div className="profile-page">
      <div className="profile-card">
        <div className="profile-card__header">
          <h1>Личный кабинет</h1>
          <p>Информация о текущем пользователе</p>
        </div>

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