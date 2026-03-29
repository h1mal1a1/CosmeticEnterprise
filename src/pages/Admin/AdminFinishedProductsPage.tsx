import './AdminPage.css';

export default function AdminFinishedProductsPage() {
  return (
    <div className="admin-page">
      <div className="admin-page__header">
        <h1>Готовая продукция</h1>
        <p>Управление готовыми продуктами</p>
      </div>

      <div className="admin-card">
        <h2>Раздел в разработке</h2>
        <p className="admin-state">
          Здесь мы следующим шагом добавим создание, редактирование и удаление
          готовой продукции.
        </p>
      </div>
    </div>
  );
}