import { useEffect, useMemo, useState } from 'react';
import { Link, useSearchParams } from 'react-router-dom';
import { getFinishedProducts } from '../../api/finishedProductsApi';
import { getCategories, type Category } from '../../api/categoriesApi';
import type { FinishedProduct } from '../../types/finishedProduct';
import './ProductsPage.css';

function getMainImage(product: FinishedProduct) {
  return product.images.find((x) => x.isMain) ?? product.images[0] ?? null;
}

export default function ProductsPage() {
  const [searchParams] = useSearchParams();
  const [products, setProducts] = useState<FinishedProduct[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  const categoryIdParam = searchParams.get('categoryId');
  const selectedCategoryId = categoryIdParam ? Number(categoryIdParam) : null;

  useEffect(() => {
    async function loadData() {
      try {
        setError('');
        setIsLoading(true);

        const [productsData, categoriesData] = await Promise.all([
          getFinishedProducts(),
          getCategories(),
        ]);

        setProducts(productsData);
        setCategories(categoriesData);
      } catch {
        setError('Не удалось загрузить продукцию');
      } finally {
        setIsLoading(false);
      }
    }

    void loadData();
  }, []);

  const selectedCategory = useMemo(() => {
    if (selectedCategoryId === null) {
      return null;
    }

    return categories.find((x) => x.id === selectedCategoryId) ?? null;
  }, [categories, selectedCategoryId]);

  const filteredProducts = useMemo(() => {
    if (selectedCategoryId === null) {
      return products;
    }

    return products.filter((x) => x.idProductCategory === selectedCategoryId);
  }, [products, selectedCategoryId]);

  if (isLoading) {
    return <div className="products-state">Загрузка продукции...</div>;
  }

  if (error) {
    return <div className="products-state products-state--error">{error}</div>;
  }

  return (
    <div className="products-page">
      <div className="products-header">
        <h1 className="products-title">
          {selectedCategory ? selectedCategory.name : 'Продукция'}
        </h1>
        <p className="products-subtitle">
          {selectedCategory
            ? `Товары категории "${selectedCategory.name}"`
            : 'Вся продукция бренда'}
        </p>
      </div>

      {filteredProducts.length === 0 ? (
        <div className="products-state">Товары не найдены.</div>
      ) : (
        <div className="products-grid">
          {filteredProducts.map((product) => {
            const mainImage = getMainImage(product);

            return (
              <Link
                key={product.id}
                to={`/products/${product.id}`}
                className="product-card"
              >
                {mainImage ? (
                  <img
                    src={mainImage.fileUrl}
                    alt={product.name}
                    className="product-card__image"
                  />
                ) : (
                  <div className="product-card__image product-card__image--placeholder">
                    Нет фото
                  </div>
                )}

                <div className="product-card__overlay" />

                <div className="product-card__body">
                  <h2 className="product-card__title">{product.name}</h2>
                  <p className="product-card__text">
                    Открыть страницу товара
                  </p>
                </div>
              </Link>
            );
          })}
        </div>
      )}
    </div>
  );
}