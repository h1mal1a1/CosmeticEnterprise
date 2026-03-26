import { BrowserRouter, Routes, Route, Link, Navigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import './App.css';
import HomePage from './pages/Home/HomePage';
import CategoriesPage from './pages/Categories/CategoriesPage';
import ProductsPage from './pages/Products/ProductsPage';
import ProfilePage from './pages/Profile/ProfilePage';
import AboutPage from './pages/About/AboutPage';
import AdminPage from './pages/Admin/AdminPage';
import LoginPage from './pages/Login/LoginPage';
import ProtectedRoute from './components/auth/ProtectedRoute';
import { useAuth } from './components/auth/AuthProvider';
import { getCategories, type Category } from './api/categoriesApi';
import ProductDetailsPage from './pages/ProductDetails/ProductDetailsPage';
import AppBreadcrumbs from './components/navigation/AppBreadcrumbs';

function AppContent() {
  const { isAuthenticated, user } = useAuth();
  const [categories, setCategories] = useState<Category[]>([]);

  const isAdmin = user?.roleName?.toLowerCase() === 'admin';

  useEffect(() => {
    async function loadCategories() {
      try {
        const data = await getCategories();
        setCategories(data);
      } catch (error) {
        console.error('Не удалось загрузить категории для меню', error);
      }
    }

    void loadCategories();
  }, []);

  return (
    <div className="page-container">
      <nav className="top-nav">
        <Link to="/" className="brand-title">
          VALMÉRIS
        </Link>

        <div className="nav-links">
          <Link to="/" className="nav-link">
            Главная
          </Link>

          <div className="nav-dropdown">
            <Link to="/categories" className="nav-link nav-link--dropdown">
              Категории
              <span className="nav-dropdown__arrow">⌄</span>
            </Link>

            <div className="nav-dropdown__menu">
              {categories.length > 0 ? (
                <>
                  {categories.map((category) => (
                    <Link
                      key={category.id}
                      to={`/products?categoryId=${category.id}`}
                      className="nav-dropdown__item"
                    >
                      {category.name}
                    </Link>
                  ))}

                  <Link
                    to="/categories"
                    className="nav-dropdown__item nav-dropdown__item--all"
                  >
                    Смотреть все
                  </Link>
                </>
              ) : (
                <div className="nav-dropdown__item nav-dropdown__item--empty">
                  Загрузка...
                </div>
              )}
            </div>
          </div>

          <Link to="/products" className="nav-link">
            Продукция
          </Link>

          <Link to="/about" className="nav-link">
            О нас
          </Link>

          {isAuthenticated && isAdmin && (
            <Link to="/admin" className="nav-link nav-link--accent">
              Панель управления
            </Link>
          )}

          {isAuthenticated ? (
            <Link to="/profile" className="nav-link nav-link--button">
              Личный кабинет
            </Link>
          ) : (
            <Link to="/login" className="nav-link nav-link--button">
              Войти
            </Link>
          )}
        </div>
      </nav>

      <AppBreadcrumbs />

      <main className="main-content">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/categories" element={<CategoriesPage />} />
          <Route path="/products" element={<ProductsPage />} />
          <Route path="/products/:id" element={<ProductDetailsPage />} />
          <Route path="/about" element={<AboutPage />} />
          <Route path="/login" element={<LoginPage />} />

          <Route
            path="/profile"
            element={
              <ProtectedRoute>
                <ProfilePage />
              </ProtectedRoute>
            }
          />

          <Route
            path="/admin"
            element={
              <ProtectedRoute>
                {isAdmin ? <AdminPage /> : <Navigate to="/" replace />}
              </ProtectedRoute>
            }
          />
        </Routes>
      </main>
    </div>
  );
}

function App() {
  return (
    <BrowserRouter>
      <AppContent />
    </BrowserRouter>
  );
}

export default App;