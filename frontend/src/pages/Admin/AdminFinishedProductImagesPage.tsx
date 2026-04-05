import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getFinishedProductById } from "../../api/finishedProductsApi";
import {
  uploadFinishedProductImage,
  deleteFinishedProductImage,
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
    const file = event.target.files?.[0];

    if (!file) {
      return;
    }

    try {
      setUploading(true);
      setError("");

      const uploadedImage = await uploadFinishedProductImage(
        finishedProductId,
        file,
      );

      setProduct((prev) => {
        if (!prev) {
          return prev;
        }

        return {
          ...prev,
          images: [...prev.images, uploadedImage],
        };
      });

      event.target.value = "";
    } catch (err) {
      console.error(err);
      setError("Не удалось загрузить изображение.");
    } finally {
      setUploading(false);
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
          <p>Загрузка и удаление изображений для готовой продукции.</p>
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
                <span>Добавить изображение</span>
                <input
                  type="file"
                  accept="image/*"
                  onChange={(event) => void handleFileChange(event)}
                  disabled={uploading}
                />
              </label>

              {uploading ? (
                <div className="admin-finished-product-images-page__hint">
                  Загрузка изображения...
                </div>
              ) : (
                <div className="admin-finished-product-images-page__hint">
                  Поддерживаются стандартные форматы изображений.
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

                    <button
                      type="button"
                      onClick={() => void handleDeleteImage(image.id)}
                      className="admin-finished-product-images-page__danger-button"
                      disabled={deletingId === image.id}
                    >
                      {deletingId === image.id ? "Удаление..." : "Удалить"}
                    </button>
                  </div>
                ))}
            </div>
          )}
        </>
      )}
    </div>
  );
}