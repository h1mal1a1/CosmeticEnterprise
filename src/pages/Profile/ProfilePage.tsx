import './ProfilePage.css';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../components/auth/AuthProvider';

export default function ProfilePage() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  async function handleLogout() {
    await logout();
    navigate('/', { replace: true });
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
              {user?.username ?? 'Неизвестно'}
            </span>
          </div>

          <div className="profile-field">
            <span className="profile-field__label">Почта</span>
            <span className="profile-field__value">Не указана</span>
          </div>
        </div>

        <button className="profile-logout-button" onClick={handleLogout}>
          Выйти
        </button>
      </div>
    </div>
  );
}