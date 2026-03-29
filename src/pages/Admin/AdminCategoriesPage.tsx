import { useEffect, useState } from 'react';
import {
  getCategories,
  createCategory,
  updateCategory,
  deleteCategory,
  type Category,
} from '../../api/categoriesApi';
import './AdminPage.css';

export default function AdminCategoriesPage() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [name, setName] = useState('');
  const [editingCategoryId, setEditingCategoryId] = useState<number | null>(null);
  const [editingCategoryName, setEditingCategoryName] = useState('');
  const [error, setError] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    void loadCategories();
  }, []);

  async function loadCategories() {
    try {
      setError('');
      const data = await getCategories();
      setCategories(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error(err);
      setError('Не удалось загрузить категории.');
      setCategories([]);
    }
  }

  async function handleCreateCategory(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const trimmedName = name.trim();

    if (!trimmedName) {
      setError('Введите название категории.');
      return;
    }

    try {
      setIsSubmitting(true);
      setError('');

      await createCategory({ name: trimmedName });

      setName('');
      await loadCategories();
    } catch (err) {
      console.error(err);
      setError('Не удалось создать категорию.');
    } finally {
      setIsSubmitting(false);
    }
  }

  function startEditing(category: Category) {
    setEditingCategoryId(category.id);
    setEditingCategoryName(category.name);
    setError('');
  }

  function cancelEditing() {
    setEditingCategoryId(null);
    setEditingCategoryName('');
  }

  async function handleUpdateCategory(id: number) {
    const trimmedName = editingCategoryName.trim();

    if (!trimmedName) {
      setError('Введите название категории.');
      return;
    }

    try {
      setIsSubmitting(true);
      setError('');

      await updateCategory(id, { name: trimmedName });

      setEditingCategoryId(null);
      setEditingCategoryName('');
      await loadCategories();
    } catch (err) {
      console.error(err);
      setError('Не удалось обновить категорию.');
    } finally {
      setIsSubmitting(false);
    }
  }

  async function handleDeleteCategory(id: number) {
    const isConfirmed = window.confirm('Удалить категорию?');

    if (!isConfirmed) {
      return;
    }

    try {
      setError('');
      await deleteCategory(id);
      await loadCategories();
    } catch (err) {
      console.error(err);
      setError('Не удалось удалить категорию.');
    }
  }

  return (
    <div className="admin-page">
      <div className="admin-page__header">
        <h1>Категории</h1>
        <p>Управление категориями товаров</p>
      </div>

      <div className="admin-layout">
        <div className="admin-card">
          <h2>Добавить категорию</h2>

          <form className="admin-form" onSubmit={handleCreateCategory}>
            <label htmlFor="category-name">Название категории</label>

            <input
              id="category-name"
              type="text"
              placeholder="Например: Крема"
              value={name}
              onChange={(event) => setName(event.target.value)}
            />

            <div className="admin-form__actions">
              <button type="submit" disabled={isSubmitting}>
                {isSubmitting ? 'Сохранение...' : 'Добавить'}
              </button>
            </div>
          </form>

          {error && <div className="admin-error">{error}</div>}
        </div>

        <div className="admin-card">
          <h2>Список категорий</h2>

          {categories.length === 0 ? (
            <div className="admin-state">Категории пока отсутствуют.</div>
          ) : (
            <div className="admin-list">
              {categories.map((category) => (
                <div key={category.id} className="admin-list__item">
                  <div>
                    {editingCategoryId === category.id ? (
                      <>
                        <input
                          type="text"
                          value={editingCategoryName}
                          onChange={(event) => setEditingCategoryName(event.target.value)}
                        />
                        <div className="admin-list__id">ID: {category.id}</div>
                      </>
                    ) : (
                      <>
                        <div className="admin-list__title">{category.name}</div>
                        <div className="admin-list__id">ID: {category.id}</div>
                      </>
                    )}
                  </div>

                  <div className="admin-list__actions">
                    {editingCategoryId === category.id ? (
                      <>
                        <button
                          type="button"
                          className="admin-button"
                          onClick={() => void handleUpdateCategory(category.id)}
                        >
                          Сохранить
                        </button>

                        <button
                          type="button"
                          className="admin-button admin-button--secondary"
                          onClick={cancelEditing}
                        >
                          Отмена
                        </button>
                      </>
                    ) : (
                      <>
                        <button
                          type="button"
                          className="admin-button admin-button--secondary"
                          onClick={() => startEditing(category)}
                        >
                          Изменить
                        </button>

                        <button
                          type="button"
                          className="admin-button admin-button--danger"
                          onClick={() => void handleDeleteCategory(category.id)}
                        >
                          Удалить
                        </button>
                      </>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}

          {error && <div className="admin-error">{error}</div>}
        </div>
      </div>
    </div>
  );
}