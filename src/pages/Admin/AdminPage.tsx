import './AdminPage.css';
import { useAuth } from '../../components/auth/AuthProvider';

const adminSections = [
  { title: 'Готовая продукция', description: 'Просмотр, создание и редактирование finished products' },
];

export default function AdminPage() {
  const { user, logout } = useAuth();

  return (
    <div className="admin-page">
      <div className="admin-header">
        <h1>Админ-панель</h1>
        <p>
          Панель для тестирования CRUD, ролей, авторизации и работы с сущностями.
        </p>
        {user && (
          <div className="admin-user-block">
            <div>Пользователь: {user.username}</div>
            <div>Роль: {user.roleName}</div>
            <button className="admin-card-button" onClick={logout}>
              Выйти
            </button>
          </div>
        )}
      </div>

      <div className="admin-grid">
        {adminSections.map((section) => (
          <div className="admin-card" key={section.title}>
            <h3>{section.title}</h3>
            <p>{section.description}</p>
            <button className="admin-card-button">Открыть</button>
          </div>
        ))}
      </div>
    </div>
  );
}