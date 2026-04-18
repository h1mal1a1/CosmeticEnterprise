import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { getCategories, type Category } from "../../api/categoriesApi";
import { getRecipes, type Recipe } from "../../api/recipesApi";
import {
  getUnitsOfMeasurement,
  type UnitOfMeasurement,
} from "../../api/unitsOfMeasurementApi";
import {
  getFinishedProducts,
  createFinishedProduct,
  updateFinishedProduct,
  deleteFinishedProduct,
} from "../../api/finishedProductsApi";
import type {
  FinishedProduct,
  CreateFinishedProductRequest,
  UpdateFinishedProductRequest,
} from "../../types/finishedProduct";
import "./AdminFinishedProductsPage.css";

type FormState = {
  name: string;
  price: string;
  idProductCategory: string;
  idRecipe: string;
  idUnitsOfMeasurement: string;
};

const initialFormState: FormState = {
  name: "",
  price: "",
  idProductCategory: "",
  idRecipe: "",
  idUnitsOfMeasurement: "",
};

export default function AdminFinishedProductsPage() {
  const [products, setProducts] = useState<FinishedProduct[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [units, setUnits] = useState<UnitOfMeasurement[]>([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [formError, setFormError] = useState("");

  const [createForm, setCreateForm] = useState<FormState>(initialFormState);
  const [isCreating, setIsCreating] = useState(false);

  const [editingId, setEditingId] = useState<number | null>(null);
  const [editForm, setEditForm] = useState<FormState>(initialFormState);
  const [isSaving, setIsSaving] = useState(false);

  const [deletingId, setDeletingId] = useState<number | null>(null);

  const categoryMap = useMemo(
    () => new Map(categories.map((item) => [item.id, item.name])),
    [categories],
  );

  const recipeMap = useMemo(
    () => new Map(recipes.map((item) => [item.id, item.name])),
    [recipes],
  );

  const unitMap = useMemo(
    () => new Map(units.map((item) => [item.id, item.name])),
    [units],
  );

  async function loadData() {
    try {
      setLoading(true);
      setError("");

      const [productsData, categoriesData, recipesData, unitsData] =
        await Promise.all([
          getFinishedProducts(),
          getCategories(),
          getRecipes(),
          getUnitsOfMeasurement(),
        ]);

      setProducts(Array.isArray(productsData) ? productsData : []);
      setCategories(Array.isArray(categoriesData) ? categoriesData : []);
      setRecipes(Array.isArray(recipesData) ? recipesData : []);
      setUnits(Array.isArray(unitsData) ? unitsData : []);
    } catch (err) {
      console.error(err);
      setError("Не удалось загрузить данные для готовой продукции.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void loadData();
  }, []);

  function handleCreateFormChange(field: keyof FormState, value: string) {
    setCreateForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  }

  function handleEditFormChange(field: keyof FormState, value: string) {
    setEditForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  }

  function validateForm(form: FormState): string {
    if (!form.name.trim()) {
      return "Введите название готовой продукции.";
    }

    if (!form.price.trim()) {
      return "Введите цену.";
    }

    const price = Number(form.price);
    if (Number.isNaN(price) || price <= 0) {
      return "Цена должна быть больше 0.";
    }

    if (!form.idProductCategory) {
      return "Выберите категорию.";
    }

    if (!form.idRecipe) {
      return "Выберите рецептуру.";
    }

    if (!form.idUnitsOfMeasurement) {
      return "Выберите единицу измерения.";
    }

    return "";
  }

  function mapFormToCreateRequest(
    form: FormState,
  ): CreateFinishedProductRequest {
    return {
      name: form.name.trim(),
      price: Number(form.price),
      idProductCategory: Number(form.idProductCategory),
      idRecipe: Number(form.idRecipe),
      idUnitsOfMeasurement: Number(form.idUnitsOfMeasurement),
    };
  }

  function mapFormToUpdateRequest(
    form: FormState,
  ): UpdateFinishedProductRequest {
    return {
      name: form.name.trim(),
      price: Number(form.price),
      idProductCategory: Number(form.idProductCategory),
      idRecipe: Number(form.idRecipe),
      idUnitsOfMeasurement: Number(form.idUnitsOfMeasurement),
    };
  }

  async function handleCreate() {
    const validationError = validateForm(createForm);

    if (validationError) {
      setFormError(validationError);
      return;
    }

    try {
      setIsCreating(true);
      setFormError("");
      setError("");

      const request = mapFormToCreateRequest(createForm);
      const createdProduct = await createFinishedProduct(request);

      setProducts((prev) => [...prev, createdProduct]);
      setCreateForm(initialFormState);
    } catch (err) {
      console.error(err);
      setFormError("Не удалось создать готовую продукцию.");
    } finally {
      setIsCreating(false);
    }
  }

  function handleStartEdit(product: FinishedProduct) {
    setEditingId(product.id);
    setFormError("");
    setError("");

    setEditForm({
      name: product.name ?? "",
      price: String(product.price ?? ""),
      idProductCategory: String(product.idProductCategory ?? ""),
      idRecipe: String(product.idRecipe ?? ""),
      idUnitsOfMeasurement: String(product.idUnitsOfMeasurement ?? ""),
    });
  }

  function handleCancelEdit() {
    setEditingId(null);
    setEditForm(initialFormState);
  }

  async function handleSaveEdit(id: number) {
    const validationError = validateForm(editForm);

    if (validationError) {
      setFormError(validationError);
      return;
    }

    try {
      setIsSaving(true);
      setFormError("");
      setError("");

      const request = mapFormToUpdateRequest(editForm);
      const updatedProduct = await updateFinishedProduct(id, request);

      setProducts((prev) =>
        prev.map((product) => (product.id === id ? updatedProduct : product)),
      );

      setEditingId(null);
      setEditForm(initialFormState);
    } catch (err) {
      console.error(err);
      setFormError("Не удалось обновить готовую продукцию.");
    } finally {
      setIsSaving(false);
    }
  }

  async function handleDelete(id: number) {
    const isConfirmed = window.confirm(
      "Удалить готовую продукцию? Это действие нельзя отменить.",
    );

    if (!isConfirmed) {
      return;
    }

    try {
      setDeletingId(id);
      setError("");
      setFormError("");

      await deleteFinishedProduct(id);
      setProducts((prev) => prev.filter((product) => product.id !== id));
    } catch (err) {
      console.error(err);
      setError("Не удалось удалить готовую продукцию.");
    } finally {
      setDeletingId(null);
    }
  }

  return (
    <div className="admin-finished-products-page">
      <div className="admin-finished-products-page__header">
        <h1>Готовая продукция</h1>
        <p>
          Создание и редактирование продукции с привязкой к категории, рецептуре
          и единице измерения.
        </p>
      </div>

      <section className="admin-finished-products-page__card">
        <h2 className="admin-finished-products-page__section-title">
          Добавить продукт
        </h2>

        <div className="admin-finished-products-page__form-grid">
          <label className="admin-finished-products-page__field">
            <span>Название</span>
            <input
              type="text"
              value={createForm.name}
              onChange={(event) =>
                handleCreateFormChange("name", event.target.value)
              }
              placeholder="Например, Nivea"
              disabled={isCreating}
            />
          </label>

          <label className="admin-finished-products-page__field">
            <span>Цена</span>
            <input
              type="number"
              min="0.01"
              step="0.01"
              value={createForm.price}
              onChange={(event) =>
                handleCreateFormChange("price", event.target.value)
              }
              placeholder="Например, 499.99"
              disabled={isCreating}
            />
          </label>

          <label className="admin-finished-products-page__field">
            <span>Категория</span>
            <select
              value={createForm.idProductCategory}
              onChange={(event) =>
                handleCreateFormChange("idProductCategory", event.target.value)
              }
              disabled={isCreating}
            >
              <option value="">Выберите категорию</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              ))}
            </select>
          </label>

          <label className="admin-finished-products-page__field">
            <span>Рецептура</span>
            <select
              value={createForm.idRecipe}
              onChange={(event) =>
                handleCreateFormChange("idRecipe", event.target.value)
              }
              disabled={isCreating}
            >
              <option value="">Выберите рецептуру</option>
              {recipes.map((recipe) => (
                <option key={recipe.id} value={recipe.id}>
                  {recipe.name}
                </option>
              ))}
            </select>
          </label>

          <label className="admin-finished-products-page__field">
            <span>Единица измерения</span>
            <select
              value={createForm.idUnitsOfMeasurement}
              onChange={(event) =>
                handleCreateFormChange(
                  "idUnitsOfMeasurement",
                  event.target.value,
                )
              }
              disabled={isCreating}
            >
              <option value="">Выберите единицу измерения</option>
              {units.map((unit) => (
                <option key={unit.id} value={unit.id}>
                  {unit.name}
                </option>
              ))}
            </select>
          </label>
        </div>

        {formError ? (
          <div className="admin-finished-products-page__error">{formError}</div>
        ) : null}

        <div className="admin-finished-products-page__form-actions">
          <button
            type="button"
            onClick={() => void handleCreate()}
            className="admin-finished-products-page__primary-button"
            disabled={isCreating}
          >
            {isCreating ? "Добавление..." : "Добавить"}
          </button>
        </div>
      </section>

      {error ? (
        <div className="admin-finished-products-page__error">{error}</div>
      ) : null}

      {loading ? (
        <div className="admin-finished-products-page__state">Загрузка...</div>
      ) : products.length === 0 ? (
        <div className="admin-finished-products-page__state">
          Список готовой продукции пуст.
        </div>
      ) : (
        <div className="admin-finished-products-page__table-wrapper">
          <table className="admin-finished-products-page__table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Название</th>
                <th>Цена</th>
                <th>Категория</th>
                <th>Рецептура</th>
                <th>Ед. изм.</th>
                <th>Действия</th>
              </tr>
            </thead>
            <tbody>
              {products.map((product) => {
                const isEditing = editingId === product.id;
                const isDeleting = deletingId === product.id;

                return (
                  <tr key={product.id}>
                    <td>{product.id}</td>

                    <td>
                      {isEditing ? (
                        <input
                          type="text"
                          value={editForm.name}
                          onChange={(event) =>
                            handleEditFormChange("name", event.target.value)
                          }
                          className="admin-finished-products-page__inline-input"
                          disabled={isSaving}
                        />
                      ) : (
                        product.name
                      )}
                    </td>

                    <td>
                      {isEditing ? (
                        <input
                          type="number"
                          min="0.01"
                          step="0.01"
                          value={editForm.price}
                          onChange={(event) =>
                            handleEditFormChange("price", event.target.value)
                          }
                          className="admin-finished-products-page__inline-input"
                          disabled={isSaving}
                        />
                      ) : (
                        product.price
                      )}
                    </td>

                    <td>
                      {isEditing ? (
                        <select
                          value={editForm.idProductCategory}
                          onChange={(event) =>
                            handleEditFormChange(
                              "idProductCategory",
                              event.target.value,
                            )
                          }
                          className="admin-finished-products-page__inline-select"
                          disabled={isSaving}
                        >
                          <option value="">Выберите категорию</option>
                          {categories.map((category) => (
                            <option key={category.id} value={category.id}>
                              {category.name}
                            </option>
                          ))}
                        </select>
                      ) : (
                        categoryMap.get(product.idProductCategory) ?? "—"
                      )}
                    </td>

                    <td>
                      {isEditing ? (
                        <select
                          value={editForm.idRecipe}
                          onChange={(event) =>
                            handleEditFormChange("idRecipe", event.target.value)
                          }
                          className="admin-finished-products-page__inline-select"
                          disabled={isSaving}
                        >
                          <option value="">Выберите рецептуру</option>
                          {recipes.map((recipe) => (
                            <option key={recipe.id} value={recipe.id}>
                              {recipe.name}
                            </option>
                          ))}
                        </select>
                      ) : (
                        recipeMap.get(product.idRecipe) ?? "—"
                      )}
                    </td>

                    <td>
                      {isEditing ? (
                        <select
                          value={editForm.idUnitsOfMeasurement}
                          onChange={(event) =>
                            handleEditFormChange(
                              "idUnitsOfMeasurement",
                              event.target.value,
                            )
                          }
                          className="admin-finished-products-page__inline-select"
                          disabled={isSaving}
                        >
                          <option value="">Выберите единицу измерения</option>
                          {units.map((unit) => (
                            <option key={unit.id} value={unit.id}>
                              {unit.name}
                            </option>
                          ))}
                        </select>
                      ) : (
                        unitMap.get(product.idUnitsOfMeasurement) ?? "—"
                      )}
                    </td>

                    <td>
                      <div className="admin-finished-products-page__actions">
                        {isEditing ? (
                          <>
                            <button
                              type="button"
                              onClick={() => void handleSaveEdit(product.id)}
                              className="admin-finished-products-page__primary-button"
                              disabled={isSaving}
                            >
                              {isSaving ? "Сохранение..." : "Сохранить"}
                            </button>
                            <button
                              type="button"
                              onClick={handleCancelEdit}
                              className="admin-finished-products-page__secondary-button"
                              disabled={isSaving}
                            >
                              Отмена
                            </button>
                          </>
                        ) : (
                          <>
                            <Link
                              to={`/admin/finished-products/${product.id}/images`}
                              className="admin-finished-products-page__secondary-link"
                            >
                              Изображения
                            </Link>

                            <button
                              type="button"
                              onClick={() => handleStartEdit(product)}
                              className="admin-finished-products-page__secondary-button"
                              disabled={deletingId !== null}
                            >
                              Редактировать
                            </button>
                            <button
                              type="button"
                              onClick={() => void handleDelete(product.id)}
                              className="admin-finished-products-page__danger-button"
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