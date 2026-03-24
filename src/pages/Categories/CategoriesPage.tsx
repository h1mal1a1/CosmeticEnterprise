import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getCategories, type Category } from '../../api/categoriesApi';
import './CategoriesPage.css';

export default function CategoriesPage() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    async function loadCategories() {
      try {
        setError('');
        const data = await getCategories();
        setCategories(data);
      } catch {
        setError('Не удалось загрузить категории');
      } finally {
        setIsLoading(false);
      }
    }

    void loadCategories();
  }, []);

  if (isLoading) {
    return <div className="categories-loading">Загрузка категорий...</div>;
  }

  if (error) {
    return <div className="categories-error">{error}</div>;
  }

  return (
    <div className="categories-page">
      <div className="categories-header">
        <h1 className="categories-title">Категории</h1>
        <p className="categories-subtitle">
          Подберите нужное направление ухода
        </p>
      </div>

      {categories.length === 0 ? (
        <div className="categories-empty">Категории пока отсутствуют.</div>
      ) : (
        <div className="categories-grid">
          {categories.map((category) => (
            <Link
              key={category.id}
              to={`/products?categoryId=${category.id}`}
              className="category-card"
            >
              <div className="category-card__content">
                <h2 className="category-card__title">{category.name}</h2>
                <p className="category-card__text">
                  Перейти к подборке товаров этой категории
                </p>
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}