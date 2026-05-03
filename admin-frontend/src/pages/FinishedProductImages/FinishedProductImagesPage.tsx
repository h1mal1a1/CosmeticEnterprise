import { useEffect, useState } from "react";
import { Button, useNotify, useRedirect } from "react-admin";
import { useParams } from "react-router-dom";
import {
  deleteFinishedProductImage,
  getFinishedProductById,
  setMainFinishedProductImage,
  uploadFinishedProductImages,
  type FinishedProduct,
  type FinishedProductImage,
} from "../../api/finishedProductImagesApi";
import "./FinishedProductImagesPage.css";

export default function FinishedProductImagesPage() {
  const { id } = useParams<{ id: string }>();
  const redirect = useRedirect();
  const notify = useNotify();

  const finishedProductId = Number(id);

  const [product, setProduct] = useState<FinishedProduct | null>(null);
  const [loading, setLoading] = useState(true);
  const [uploading, setUploading] = useState(false);
  const [deletingId, setDeletingId] = useState<number | null>(null);
  const [settingMainId, setSettingMainId] = useState<number | null>(null);

  useEffect(() => {
    if (!Number.isFinite(finishedProductId)) {
      notify("Некорректный идентификатор продукта", { type: "error" });
      setLoading(false);
      return;
    }

    void loadProduct();
  }, [finishedProductId]);

  async function loadProduct() {
    try {
      setLoading(true);
      const data = await getFinishedProductById(finishedProductId);
      setProduct(data);
    } catch (error) {
      console.error(error);
      notify("Не удалось загрузить продукт", { type: "error" });
    } finally {
      setLoading(false);
    }
  }

  async function handleFileChange(event: React.ChangeEvent<HTMLInputElement>) {
    const files = Array.from(event.target.files ?? []);

    if (files.length === 0) {
      return;
    }

    try {
      setUploading(true);

      const uploadedImages = await uploadFinishedProductImages(
        finishedProductId,
        files,
      );

      setProduct((current) => {
        if (!current) {
          return current;
        }

        return {
          ...current,
          images: [...current.images, ...uploadedImages],
        };
      });

      event.target.value = "";
      notify("Изображения загружены", { type: "success" });
    } catch (error) {
      console.error(error);
      notify("Не удалось загрузить изображения", { type: "error" });
    } finally {
      setUploading(false);
    }
  }

  async function handleSetMain(imageId: number) {
    try {
      setSettingMainId(imageId);

      const updatedImages = await setMainFinishedProductImage(
        finishedProductId,
        imageId,
      );

      setProduct((current) =>
        current ? { ...current, images: updatedImages } : current,
      );

      notify("Главное изображение обновлено", { type: "success" });
    } catch (error) {
      console.error(error);
      notify("Не удалось установить главное изображение", { type: "error" });
    } finally {
      setSettingMainId(null);
    }
  }

  async function handleDeleteImage(imageId: number) {
    const confirmed = window.confirm(
      "Удалить изображение? Это действие нельзя отменить.",
    );

    if (!confirmed) {
      return;
    }

    try {
      setDeletingId(imageId);

      await deleteFinishedProductImage(finishedProductId, imageId);

      setProduct((current) => {
        if (!current) {
          return current;
        }

        return {
          ...current,
          images: current.images.filter((image) => image.id !== imageId),
        };
      });

      notify("Изображение удалено", { type: "success" });
    } catch (error) {
      console.error(error);
      notify("Не удалось удалить изображение", { type: "error" });
    } finally {
      setDeletingId(null);
    }
  }

  function renderImageBadge(image: FinishedProductImage) {
    if (!image.isMain) {
      return null;
    }

    return (
      <span className="finished-product-images-page__badge">Главное</span>
    );
  }

  if (loading) {
    return (
      <div className="finished-product-images-page">
        <p className="finished-product-images-page__state">Загрузка...</p>
      </div>
    );
  }

  if (!product) {
    return (
      <div className="finished-product-images-page">
        <Button
          label="Назад к готовой продукции"
          onClick={() => redirect("/finished-products")}
        />
        <p className="finished-product-images-page__state">
          Продукт не найден.
        </p>
      </div>
    );
  }

  return (
    <div className="finished-product-images-page">
      <div className="finished-product-images-page__header">
        <div>
          <h1>Изображения продукта</h1>
          <p>
            Товар: <strong>{product.name}</strong>
          </p>
          <p>ID продукта: {product.id}</p>
        </div>

        <Button
          label="Назад к готовой продукции"
          onClick={() => redirect("/finished-products")}
        />
      </div>

      <section className="finished-product-images-page__upload-card">
        <label className="finished-product-images-page__upload-label">
          <span>Добавить изображения</span>
          <input
            type="file"
            accept="image/*"
            multiple
            onChange={(event) => void handleFileChange(event)}
            disabled={uploading}
          />
        </label>

        <p className="finished-product-images-page__hint">
          Можно выбрать сразу несколько изображений. Поддерживаются JPG, PNG и
          WEBP.
        </p>

        {uploading && (
          <p className="finished-product-images-page__hint">
            Загрузка изображений...
          </p>
        )}
      </section>

      {product.images.length === 0 ? (
        <p className="finished-product-images-page__state">
          У продукта пока нет изображений.
        </p>
      ) : (
        <div className="finished-product-images-page__grid">
          {product.images
            .slice()
            .sort((a, b) => a.sortOrder - b.sortOrder)
            .map((image) => (
              <article
                key={image.id}
                className="finished-product-images-page__image-card"
              >
                <div className="finished-product-images-page__image-wrapper">
                  <img
                    src={image.fileUrl}
                    alt={product.name}
                    className="finished-product-images-page__image"
                  />
                </div>

                <div className="finished-product-images-page__meta">
                  <div className="finished-product-images-page__meta-row">
                    <span>ID: {image.id}</span>
                    {renderImageBadge(image)}
                  </div>

                  <div className="finished-product-images-page__meta-row">
                    <span>Sort order: {image.sortOrder}</span>
                  </div>
                </div>

                <div className="finished-product-images-page__actions">
                  <button
                    type="button"
                    className="finished-product-images-page__primary-button"
                    onClick={() => void handleSetMain(image.id)}
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
                    className="finished-product-images-page__danger-button"
                    onClick={() => void handleDeleteImage(image.id)}
                    disabled={deletingId === image.id}
                  >
                    {deletingId === image.id ? "Удаление..." : "Удалить"}
                  </button>
                </div>
              </article>
            ))}
        </div>
      )}
    </div>
  );
}