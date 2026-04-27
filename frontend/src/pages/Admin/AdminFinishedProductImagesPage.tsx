import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getFinishedProductById } from "../../api/finishedProductsApi";
import {
  uploadFinishedProductImages,
  deleteFinishedProductImage,
  setMainFinishedProductImage,
} from "../../api/finishedProductImagesApi";
import type {
  FinishedProduct,
  FinishedProductImage,
} from "../../types/finishedProduct";
import "./AdminFinishedProductImagesPage.css";

export default function AdminFinishedProductImagesPage() {
  const params = useParams();
  const finishedProductId = Number(params.id);

  const [product, setProduct] = useState<FinishedProduct | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [uploading, setUploading] = useState(false);
  const [deletingId, setDeletingId] = useState<number | null>(null);
  const [settingMainId, setSettingMainId] = useState<number | null>(null);

  async function loadProduct() {
    try {
      setLoading(true);
      setError("");
      const data = await getFinishedProductById(finishedProductId);
      setProduct(data);
    } catch (err) {
      console.error(err);
      setError("Не удалось загрузить данные продукта.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    if (!Number.isFinite(finishedProductId)) {
      setError("Некорректный идентификатор продукта.");
      setLoading(false);
      return;
    }

    void loadProduct();
  }, [finishedProductId]);

  async function handleFileChange(event: React.ChangeEvent<HTMLInputElement>) {
    const files = Array.from(event.target.files ?? []);

    if (files.length === 0) {
      return;
    }

    try {
      setUploading(true);
      setError("");

      const uploadedImages = await uploadFinishedProductImages(
        finishedProductId,
        files,
      );

      setProduct((prev) => {
        if (!prev) {
          return prev;
        }

        return {
          ...prev,
          images: [...prev.images, ...uploadedImages],
        };
      });

      event.target.value = "";
    } catch (err) {
      console.error(err);
      setError("Не удалось загрузить изображения.");
    } finally {
      setUploading(false);
    }
  }

  async function handleSetMain(imageId: number) {
    try {
      setSettingMainId(imageId);
      setError("");

      const updatedImages = await setMainFinishedProductImage(
        finishedProductId,
        imageId,
      );

      setProduct((prev) =>
        prev ? { ...prev, images: updatedImages } : prev,
      );
    } catch (err) {
      console.error(err);
      setError("Не удалось установить главное изображение.");
    } finally {
      setSettingMainId(null);
    }
  }

  async function handleDeleteImage(imageId: number) {
    const isConfirmed = window.confirm(
      "Удалить изображение? Это действие нельзя отменить.",
    );

    if (!isConfirmed) {
      return;
    }

    try {
      setDeletingId(imageId);
      setError("");

      await deleteFinishedProductImage(finishedProductId, imageId);

      setProduct((prev) => {
        if (!prev) {
          return prev;
        }

        return {
          ...prev,
          images: prev.images.filter((image) => image.id !== imageId),
        };
      });
    } catch (err) {
      console.error(err);
      setError("Не удалось удалить изображение.");
    } finally {
      setDeletingId(null);
    }
  }

  function renderImageBadge(image: FinishedProductImage) {
    if (image.isMain) {
      return (
        <span className="admin-finished-product-images-page__badge">
          Главное
        </span>
      );
    }

    return null;
  }

  return (
    <div className="admin-finished-product-images-page">
      <div className="admin-finished-product-images-page__header">
        <div>
          <h1>Изображения продукта</h1>
          <p>Загрузка и управление изображениями готовой продукции.</p>
        </div>

        <Link
          to="/admin/finished-products"
          className="admin-finished-product-images-page__back-link"
        >
          ← Назад к продукции
        </Link>
      </div>

      {error ? (
        <div className="admin-finished-product-images-page__error">{error}</div>
      ) : null}

      {loading ? (
        <div className="admin-finished-product-images-page__state">
          Загрузка...
        </div>
      ) : !product ? (
        <div className="admin-finished-product-images-page__state">
          Продукт не найден.
        </div>
      ) : (
        <>
          <section className="admin-finished-product-images-page__card">
            <h2 className="admin-finished-product-images-page__title">
              {product.name}
            </h2>

            <div className="admin-finished-product-images-page__upload">
              <label className="admin-finished-product-images-page__upload-label">
                <span>Добавить изображения</span>
                <input
                  type="file"
                  accept="image/*"
                  multiple
                  onChange={(event) => void handleFileChange(event)}
                  disabled={uploading}
                />
              </label>

              {uploading ? (
                <div className="admin-finished-product-images-page__hint">
                  Загрузка изображений...
                </div>
              ) : (
                <div className="admin-finished-product-images-page__hint">
                  Можно выбрать сразу несколько изображений. Поддерживаются JPG,
                  PNG и WEBP.
                </div>
              )}
            </div>
          </section>

          {product.images.length === 0 ? (
            <div className="admin-finished-product-images-page__state">
              У продукта пока нет изображений.
            </div>
          ) : (
            <div className="admin-finished-product-images-page__grid">
              {product.images
                .slice()
                .sort((a, b) => a.sortOrder - b.sortOrder)
                .map((image) => (
                  <div
                    key={image.id}
                    className="admin-finished-product-images-page__image-card"
                  >
                    <div className="admin-finished-product-images-page__image-wrapper">
                      <img
                        src={image.fileUrl}
                        alt={product.name}
                        className="admin-finished-product-images-page__image"
                      />
                    </div>

                    <div className="admin-finished-product-images-page__meta">
                      <div className="admin-finished-product-images-page__meta-row">
                        <span>ID: {image.id}</span>
                        {renderImageBadge(image)}
                      </div>
                      <div className="admin-finished-product-images-page__meta-row">
                        Sort order: {image.sortOrder}
                      </div>
                    </div>

                    <div className="admin-finished-product-images-page__actions">
                      <button
                        type="button"
                        onClick={() => void handleSetMain(image.id)}
                        className="admin-finished-product-images-page__primary-button"
                        disabled={image.isMain || settingMainId === image.id}
                      >
                        {image.isMain
                          ? "Главное"
                          : settingMainId === image.id
                          ? "Сохранение..."
                          : "Сделать главным"}
                      </button>

                      <button
                        type="button"
                        onClick={() => void handleDeleteImage(image.id)}
                        className="admin-finished-product-images-page__danger-button"
                        disabled={deletingId === image.id}
                      >
                        {deletingId === image.id ? "Удаление..." : "Удалить"}
                      </button>
                    </div>
                  </div>
                ))}
            </div>
          )}
        </>
      )}
    </div>
  );
}