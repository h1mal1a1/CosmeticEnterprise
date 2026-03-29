import { useEffect, useState } from "react";
import {
  createUnitOfMeasurement,
  deleteUnitOfMeasurement,
  getUnitsOfMeasurement,
  updateUnitOfMeasurement,
  type UnitOfMeasurement,
} from "../../api/unitsOfMeasurementApi";
import "./AdminDictionaryPage.css";

export default function AdminUnitsOfMeasurementPage() {
  const [units, setUnits] = useState<UnitOfMeasurement[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>("");
  const [createName, setCreateName] = useState("");
  const [isCreating, setIsCreating] = useState(false);

  const [editingId, setEditingId] = useState<number | null>(null);
  const [editingName, setEditingName] = useState("");
  const [isSaving, setIsSaving] = useState(false);
  const [deletingId, setDeletingId] = useState<number | null>(null);

  async function loadUnits() {
    try {
      setLoading(true);
      setError("");
      const data = await getUnitsOfMeasurement();
      setUnits(data);
    } catch (err) {
      console.error(err);
      setError("Не удалось загрузить единицы измерения.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void loadUnits();
  }, []);

  async function handleCreate() {
    const name = createName.trim();

    if (!name) {
      setError("Введите название единицы измерения.");
      return;
    }

    try {
      setIsCreating(true);
      setError("");
      const created = await createUnitOfMeasurement({ name });
      setUnits((prev) => [...prev, created]);
      setCreateName("");
    } catch (err) {
      console.error(err);
      setError("Не удалось создать единицу измерения.");
    } finally {
      setIsCreating(false);
    }
  }

  function handleStartEdit(unit: UnitOfMeasurement) {
    setEditingId(unit.id);
    setEditingName(unit.name);
    setError("");
  }

  function handleCancelEdit() {
    setEditingId(null);
    setEditingName("");
  }

  async function handleSaveEdit(id: number) {
    const name = editingName.trim();

    if (!name) {
      setError("Название единицы измерения не может быть пустым.");
      return;
    }

    try {
      setIsSaving(true);
      setError("");
      await updateUnitOfMeasurement(id, { name });

      setUnits((prev) =>
        prev.map((unit) =>
          unit.id === id ? { ...unit, name } : unit,
        ),
      );

      setEditingId(null);
      setEditingName("");
    } catch (err) {
      console.error(err);
      setError("Не удалось обновить единицу измерения.");
    } finally {
      setIsSaving(false);
    }
  }

  async function handleDelete(id: number) {
    const isConfirmed = window.confirm(
      "Удалить единицу измерения? Это действие нельзя отменить.",
    );

    if (!isConfirmed) {
      return;
    }

    try {
      setDeletingId(id);
      setError("");
      await deleteUnitOfMeasurement(id);
      setUnits((prev) => prev.filter((unit) => unit.id !== id));
    } catch (err) {
      console.error(err);
      setError("Не удалось удалить единицу измерения. Возможно, она уже используется.");
    } finally {
      setDeletingId(null);
    }
  }

  return (
    <div className="admin-dictionary-page">
      <div className="admin-dictionary-page__header">
        <h1>Единицы измерения</h1>
        <p>Управление справочником единиц измерения.</p>
      </div>

      <div className="admin-dictionary-page__create">
        <input
          type="text"
          value={createName}
          onChange={(event) => setCreateName(event.target.value)}
          placeholder="Введите единицу измерения"
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
      ) : units.length === 0 ? (
        <div className="admin-dictionary-page__state">Список единиц измерения пуст.</div>
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
              {units.map((unit) => {
                const isEditing = editingId === unit.id;
                const isDeleting = deletingId === unit.id;

                return (
                  <tr key={unit.id}>
                    <td>{unit.id}</td>
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
                        unit.name
                      )}
                    </td>
                    <td>
                      <div className="admin-dictionary-page__actions">
                        {isEditing ? (
                          <>
                            <button
                              type="button"
                              onClick={() => void handleSaveEdit(unit.id)}
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
                              onClick={() => handleStartEdit(unit)}
                              className="admin-dictionary-page__secondary-button"
                              disabled={deletingId !== null}
                            >
                              Редактировать
                            </button>
                            <button
                              type="button"
                              onClick={() => void handleDelete(unit.id)}
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