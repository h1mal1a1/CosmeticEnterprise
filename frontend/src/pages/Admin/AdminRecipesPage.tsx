import { useEffect, useState } from "react";
import {
  createRecipe,
  deleteRecipe,
  getRecipes,
  updateRecipe,
  type Recipe,
} from "../../api/recipesApi";
import "./AdminDictionaryPage.css";

export default function AdminRecipesPage() {
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>("");
  const [createName, setCreateName] = useState("");
  const [isCreating, setIsCreating] = useState(false);

  const [editingId, setEditingId] = useState<number | null>(null);
  const [editingName, setEditingName] = useState("");
  const [isSaving, setIsSaving] = useState(false);
  const [deletingId, setDeletingId] = useState<number | null>(null);

  async function loadRecipes() {
    try {
      setLoading(true);
      setError("");
      const data = await getRecipes();
      setRecipes(data);
    } catch (err) {
      console.error(err);
      setError("Не удалось загрузить рецептуры.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void loadRecipes();
  }, []);

  async function handleCreate() {
    const name = createName.trim();

    if (!name) {
      setError("Введите название рецептуры.");
      return;
    }

    try {
      setIsCreating(true);
      setError("");
      const created = await createRecipe({ name });
      setRecipes((prev) => [...prev, created]);
      setCreateName("");
    } catch (err) {
      console.error(err);
      setError("Не удалось создать рецептуру.");
    } finally {
      setIsCreating(false);
    }
  }

  function handleStartEdit(recipe: Recipe) {
    setEditingId(recipe.id);
    setEditingName(recipe.name);
    setError("");
  }

  function handleCancelEdit() {
    setEditingId(null);
    setEditingName("");
  }

  async function handleSaveEdit(id: number) {
    const name = editingName.trim();

    if (!name) {
      setError("Название рецептуры не может быть пустым.");
      return;
    }

    try {
      setIsSaving(true);
      setError("");
      await updateRecipe(id, { name });

      setRecipes((prev) =>
        prev.map((recipe) =>
          recipe.id === id ? { ...recipe, name } : recipe,
        ),
      );

      setEditingId(null);
      setEditingName("");
    } catch (err) {
      console.error(err);
      setError("Не удалось обновить рецептуру.");
    } finally {
      setIsSaving(false);
    }
  }

  async function handleDelete(id: number) {
    const isConfirmed = window.confirm(
      "Удалить рецептуру? Это действие нельзя отменить.",
    );

    if (!isConfirmed) {
      return;
    }

    try {
      setDeletingId(id);
      setError("");
      await deleteRecipe(id);
      setRecipes((prev) => prev.filter((recipe) => recipe.id !== id));
    } catch (err) {
      console.error(err);
      setError("Не удалось удалить рецептуру. Возможно, она уже используется.");
    } finally {
      setDeletingId(null);
    }
  }

  return (
    <div className="admin-dictionary-page">
      <div className="admin-dictionary-page__header">
        <h1>Рецептуры</h1>
        <p>Управление справочником рецептур для готовой продукции.</p>
      </div>

      <div className="admin-dictionary-page__create">
        <input
          type="text"
          value={createName}
          onChange={(event) => setCreateName(event.target.value)}
          placeholder="Введите название рецептуры"
          className="admin-dictionary-page__input"
          disabled={isCreating}
        />
        <button
          type="button"
          onClick={() => void handleCreate()}
          className="admin-dictionary-page__primary-button"
          disabled={isCreating}
        >
          {isCreating ? "Добавление..." : "Добавить"}
        </button>
      </div>

      {error ? <div className="admin-dictionary-page__error">{error}</div> : null}

      {loading ? (
        <div className="admin-dictionary-page__state">Загрузка...</div>
      ) : recipes.length === 0 ? (
        <div className="admin-dictionary-page__state">Список рецептур пуст.</div>
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
              {recipes.map((recipe) => {
                const isEditing = editingId === recipe.id;
                const isDeleting = deletingId === recipe.id;

                return (
                  <tr key={recipe.id}>
                    <td>{recipe.id}</td>
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
                        recipe.name
                      )}
                    </td>
                    <td>
                      <div className="admin-dictionary-page__actions">
                        {isEditing ? (
                          <>
                            <button
                              type="button"
                              onClick={() => void handleSaveEdit(recipe.id)}
                              className="admin-dictionary-page__primary-button"
                              disabled={isSaving}
                            >
                              {isSaving ? "Сохранение..." : "Сохранить"}
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
                              onClick={() => handleStartEdit(recipe)}
                              className="admin-dictionary-page__secondary-button"
                              disabled={deletingId !== null}
                            >
                              Редактировать
                            </button>
                            <button
                              type="button"
                              onClick={() => void handleDelete(recipe.id)}
                              className="admin-dictionary-page__danger-button"
                              disabled={isDeleting}
                            >
                              {isDeleting ? "Удаление..." : "Удалить"}
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