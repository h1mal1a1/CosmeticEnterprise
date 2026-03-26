import { useEffect, useMemo, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { getFinishedProductById } from '../../api/finishedProductsApi';
import { getCategories, type Category } from '../../api/categoriesApi';
import type { FinishedProduct, FinishedProductImage } from '../../types/finishedProduct';
import './ProductDetailsPage.css';

function getInitialImage(product: FinishedProduct): FinishedProductImage | null {
  return product.images.find((x) => x.isMain) ?? product.images[0] ?? null;
}

export default function ProductDetailsPage() {
  const { id } = useParams();
  const productId = id ? Number(id) : null;

  const [product, setProduct] = useState<FinishedProduct | null>(null);
  const [categories, setCategories] = useState<Category[]>([]);
  const [selectedImage, setSelectedImage] = useState<FinishedProductImage | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    async function loadData() {
      if (!productId || Number.isNaN(productId)) {
        setError('Некорректный идентификатор товара');
        setIsLoading(false);
        return;
      }

      try {
        setError('');
        setIsLoading(true);

        const [productData, categoriesData] = await Promise.all([
          getFinishedProductById(productId),
          getCategories(),
        ]);

        setProduct(productData);
        setCategories(categoriesData);
        setSelectedImage(getInitialImage(productData));
      } catch {
        setError('Не удалось загрузить страницу товара');
      } finally {
        setIsLoading(false);
      }
    }

    void loadData();
  }, [productId]);

  const categoryName = useMemo(() => {
    if (!product) {
      return '';
    }

    return categories.find((x) => x.id === product.idProductCategory)?.name ?? '';
  }, [categories, product]);

  if (isLoading) {
    return <div className="product-details-state">Загрузка товара...</div>;
  }

  if (error || !product) {
    return (
      <div className="product-details-state product-details-state--error">
        {error || 'Товар не найден'}
      </div>
    );
  }

  return (
    <div className="product-details-page">
      <div className="product-details__back">
        <Link
          to={
            product.idProductCategory
              ? `/products?categoryId=${product.idProductCategory}`
              : '/products'
          }
          className="product-details__back-link"
        >
          ← Назад к товарам
        </Link>
      </div>

      <div className="product-details-card">
        <div className="product-details-gallery">
          <div className="product-details-gallery__main">
            {selectedImage ? (
              <img
                src={selectedImage.fileUrl}
                alt={product.name}
                className="product-details-gallery__main-image"
              />
            ) : (
              <div className="product-details-gallery__placeholder">
                Нет фото
              </div>
            )}
          </div>

          {product.images.length > 1 && (
            <div className="product-details-gallery__thumbs">
              {product.images
                .slice()
                .sort((a, b) => a.sortOrder - b.sortOrder)
                .map((image) => (
                  <button
                    key={image.id}
                    type="button"
                    className={
                      selectedImage?.id === image.id
                        ? 'product-details-gallery__thumb product-details-gallery__thumb--active'
                        : 'product-details-gallery__thumb'
                    }
                    onClick={() => setSelectedImage(image)}
                  >
                    <img
                      src={image.fileUrl}
                      alt={product.name}
                      className="product-details-gallery__thumb-image"
                    />
                  </button>
                ))}
            </div>
          )}
        </div>

        <div className="product-details-info">
          <div className="product-details-info__badge">
            {categoryName || 'Продукция'}
          </div>

          <h1 className="product-details-info__title">{product.name}</h1>

          <div className="product-details-info__meta">
            <div className="product-details-info__meta-item">
              <span className="product-details-info__meta-label">Категория</span>
              <span className="product-details-info__meta-value">
                {categoryName || 'Не указана'}
              </span>
            </div>

            <div className="product-details-info__meta-item">
              <span className="product-details-info__meta-label">ID товара</span>
              <span className="product-details-info__meta-value">
                {product.id}
              </span>
            </div>
          </div>

          <div className="product-details-info__description">
            <h2>О товаре</h2>
            <p>
              Здесь будет подробное описание товара, его свойства,
              преимущества и рекомендации по использованию.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}