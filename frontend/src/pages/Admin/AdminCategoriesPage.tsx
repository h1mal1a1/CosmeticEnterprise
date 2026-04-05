import { useEffect, useState } from 'react';
import {
  createCategory,
  deleteCategory,
  getCategories,
  updateCategory,
  type Category,
} from '../../api/categoriesApi';
import './AdminDictionaryPage.css';

export default function AdminCategoriesPage() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const [createName, setCreateName] = useState('');
  const [isCreating, setIsCreating] = useState(false);

  const [editingId, setEditingId] = useState<number | null>(null);
  const [editingName, setEditingName] = useState('');
  const [isSaving, setIsSaving] = useState(false);
  const [deletingId, setDeletingId] = useState<number | null>(null);

  async function loadCategories() {
    try {
      setLoading(true);
      setError('');
      const data = await getCategories();
      setCategories(data);
    } catch (err) {
      console.error(err);
      setError('Не удалось загрузить категории.');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void loadCategories();
  }, []);

  async function handleCreate() {
    const name = createName.trim();

    if (!name) {
      setError('Введите название категории.');
      return;
    }

    try {
      setIsCreating(true);
      setError('');
      const created = await createCategory({ name });
      setCategories((prev) => [...prev, created]);
      setCreateName('');
    } catch (err) {
      console.error(err);
      setError('Не удалось создать категорию.');
    } finally {
      setIsCreating(false);
    }
  }

  function handleStartEdit(category: Category) {
    setEditingId(category.id);
    setEditingName(category.name);
    setError('');
  }

  function handleCancelEdit() {
    setEditingId(null);
    setEditingName('');
  }

  async function handleSaveEdit(id: number) {
    const name = editingName.trim();

    if (!name) {
      setError('Название категории не может быть пустым.');
      return;
    }

    try {
      setIsSaving(true);
      setError('');
      await updateCategory(id, { name });

      setCategories((prev) =>
        prev.map((category) =>
          category.id === id ? { ...category, name } : category,
        ),
      );

      setEditingId(null);
      setEditingName('');
    } catch (err) {
      console.error(err);
      setError('Не удалось обновить категорию.');
    } finally {
      setIsSaving(false);
    }
  }

  async function handleDelete(id: number) {
    const isConfirmed = window.confirm(
      'Удалить категорию? Это действие нельзя отменить.',
    );

    if (!isConfirmed) {
      return;
    }

    try {
      setDeletingId(id);
      setError('');
      await deleteCategory(id);
      setCategories((prev) => prev.filter((category) => category.id !== id));
    } catch (err) {
      console.error(err);
      setError('Не удалось удалить категорию.');
    } finally {
      setDeletingId(null);
    }
  }

  return (
    <div className="admin-dictionary-page">
      <div className="admin-dictionary-page__header">
        <h1>Категории</h1>
        <p>Управление категориями товаров.</p>
      </div>

      <div className="admin-dictionary-page__create">
        <input
          type="text"
          value={createName}
          onChange={(event) => setCreateName(event.target.value)}
          placeholder="Введите название категории"
          className="admin-dictionary-page__input"
          disabled={isCreating}
        />
        <button
          type="button"
          onClick={() => void handleCreate()}
          className="admin-dictionary-page__primary-button"
          disabled={isCreating}
        >
          {isCreating ? 'Добавление...' : 'Добавить'}
        </button>
      </div>

      {error ? <div className="admin-dictionary-page__error">{error}</div> : null}

      {loading ? (
        <div className="admin-dictionary-page__state">Загрузка...</div>
      ) : categories.length === 0 ? (
        <div className="admin-dictionary-page__state">Список категорий пуст.</div>
      ) : (
        <div className="admin-dictionary-page__table-wrapper">
          <table className="admin-dictionary-page__table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Название</th>
                <th>Действия</th>
              </tr>
            </thead>
            <tbody>
              {categories.map((category) => {
                const isEditing = editingId === category.id;
                const isDeleting = deletingId === category.id;

                return (
                  <tr key={category.id}>
                    <td>{category.id}</td>
                    <td>
                      {isEditing ? (
                        <input
                          type="text"
                          value={editingName}
                          onChange={(event) => setEditingName(event.target.value)}
                          className="admin-dictionary-page__input admin-dictionary-page__input--inline"
                          disabled={isSaving}
                        />
                      ) : (
                        category.name
                      )}
                    </td>
                    <td>
                      <div className="admin-dictionary-page__actions">
                        {isEditing ? (
                          <>
                            <button
                              type="button"
                              onClick={() => void handleSaveEdit(category.id)}
                              className="admin-dictionary-page__primary-button"
                              disabled={isSaving}
                            >
                              {isSaving ? 'Сохранение...' : 'Сохранить'}
                            </button>
                            <button
                              type="button"
                              onClick={handleCancelEdit}
                              className="admin-dictionary-page__secondary-button"
                              disabled={isSaving}
                            >
                              Отмена
                            </button>
                          </>
                        ) : (
                          <>
                            <button
                              type="button"
                              onClick={() => handleStartEdit(category)}
                              className="admin-dictionary-page__secondary-button"
                              disabled={deletingId !== null}
                            >
                              Редактировать
                            </button>
                            <button
                              type="button"
                              onClick={() => void handleDelete(category.id)}
                              className="admin-dictionary-page__danger-button"
                              disabled={isDeleting}
                            >
                              {isDeleting ? 'Удаление...' : 'Удалить'}
                            </button>
                          </>
                        )}
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}