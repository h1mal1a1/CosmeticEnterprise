import { useEffect, useState, type FormEvent } from 'react';
import {
  getCategories,
  createCategory,
  updateCategory,
  deleteCategory,
  type Category,
} from '../../api/categoriesApi';
import './AdminPage.css';

export default function AdminPage() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState('');
  const [name, setName] = useState('');
  const [editingCategory, setEditingCategory] = useState<Category | null>(null);

  useEffect(() => {
    void loadCategories();
  }, []);

  async function loadCategories() {
    try {
      setError('');
      setIsLoading(true);
      const data = await getCategories();
      setCategories(data);
    } catch {
      setError('Не удалось загрузить категории');
    } finally {
      setIsLoading(false);
    }
  }

  async function handleSubmit(e: FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const trimmedName = name.trim();

    if (!trimmedName) {
      setError('Введите название категории');
      return;
    }

    try {
      setError('');
      setIsSaving(true);

      if (editingCategory) {
        await updateCategory(editingCategory.id, { name: trimmedName });
      } else {
        await createCategory({ name: trimmedName });
      }

      setName('');
      setEditingCategory(null);
      await loadCategories();
    } catch {
      setError('Не удалось сохранить категорию');
    } finally {
      setIsSaving(false);
    }
  }

  function handleEdit(category: Category) {
    setEditingCategory(category);
    setName(category.name);
    setError('');
  }

  function handleCancelEdit() {
    setEditingCategory(null);
    setName('');
    setError('');
  }

  async function handleDelete(category: Category) {
    const confirmed = window.confirm(
      `Удалить категорию "${category.name}"?`
    );

    if (!confirmed) {
      return;
    }

    try {
      setError('');
      await deleteCategory(category.id);

      if (editingCategory?.id === category.id) {
        handleCancelEdit();
      }

      await loadCategories();
    } catch {
      setError('Не удалось удалить категорию');
    }
  }

  return (
    <div className="admin-page">
      <div className="admin-page__header">
        <h1>Панель управления</h1>
        <p>Управление категориями товаров</p>
      </div>

      <div className="admin-layout">
        <section className="admin-card">
          <h2>{editingCategory ? 'Редактировать категорию' : 'Добавить категорию'}</h2>

          <form className="admin-form" onSubmit={handleSubmit}>
            <label htmlFor="category-name">Название категории</label>
            <input
              id="category-name"
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Например: Крема"
            />

            {error && <div className="admin-error">{error}</div>}

            <div className="admin-form__actions">
              <button type="submit" disabled={isSaving}>
                {isSaving
                  ? 'Сохранение...'
                  : editingCategory
                    ? 'Обновить'
                    : 'Добавить'}
              </button>

              {editingCategory && (
                <button
                  type="button"
                  className="admin-button admin-button--secondary"
                  onClick={handleCancelEdit}
                >
                  Отмена
                </button>
              )}
            </div>
          </form>
        </section>

        <section className="admin-card">
          <h2>Список категорий</h2>

          {isLoading ? (
            <div className="admin-state">Загрузка...</div>
          ) : categories.length === 0 ? (
            <div className="admin-state">Категорий пока нет</div>
          ) : (
            <div className="admin-list">
              {categories.map((category) => (
                <div key={category.id} className="admin-list__item">
                  <div className="admin-list__info">
                    <div className="admin-list__title">{category.name}</div>
                    <div className="admin-list__id">ID: {category.id}</div>
                  </div>

                  <div className="admin-list__actions">
                    <button
                      type="button"
                      className="admin-button admin-button--secondary"
                      onClick={() => handleEdit(category)}
                    >
                      Изменить
                    </button>

                    <button
                      type="button"
                      className="admin-button admin-button--danger"
                      onClick={() => handleDelete(category)}
                    >
                      Удалить
                    </button>
                  </div>
                </div>
              ))}
            </div>
          )}
        </section>
      </div>
    </div>
  );
}