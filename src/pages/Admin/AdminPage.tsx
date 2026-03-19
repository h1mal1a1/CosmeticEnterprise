import './AdminPage.css';

const adminSections = [
  { title: 'Готовая продукция', description: 'Просмотр, создание и редактирование finished products' },
  { title: 'Категории', description: 'Управление категориями продукции' },
  { title: 'Материалы', description: 'Справочник сырья и материалов' },
  { title: 'Рецептуры', description: 'Работа с рецептами и составом продукции' },
  { title: 'Склады', description: 'Список складов и управление остатками' },
  { title: 'Партии продукции', description: 'Учет партий и движения товаров' },
  { title: 'Пользователи', description: 'Просмотр ролей и доступов пользователей' },
  { title: 'Заказы', description: 'Управление заказами интернет-магазина' },
];

export default function AdminPage() {
  return (
    <div className="admin-page">
      <div className="admin-header">
        <h1>Админ-панель</h1>
        <p>
          Панель для тестирования CRUD, ролей, авторизации и работы с сущностями.
        </p>
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