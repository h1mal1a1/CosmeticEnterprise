import { useEffect, useMemo, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { getFinishedProducts } from '../../api/finishedProductsApi';
import { getCategories, type Category } from '../../api/categoriesApi';
import type { FinishedProduct } from '../../types/finishedProduct';
import './ProductsPage.css';

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
          {filteredProducts.map((product) => (
            <div key={product.id} className="product-card">
              <div className="product-card__body">
                <div className="product-card__meta">
                  Категория ID: {product.idProductCategory}
                </div>
                <h2 className="product-card__title">{product.name}</h2>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}