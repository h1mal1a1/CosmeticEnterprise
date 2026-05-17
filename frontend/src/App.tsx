import { BrowserRouter, Routes, Route, Link, Navigate } from "react-router-dom";
import { useEffect, useState } from "react";
import "./App.css";
import HomePage from "./pages/Home/HomePage";
import CategoriesPage from "./pages/Categories/CategoriesPage";
import ProductsPage from "./pages/Products/ProductsPage";
import ProfilePage from "./pages/Profile/ProfilePage";
import AboutPage from "./pages/About/AboutPage";
import LoginPage from "./pages/Login/LoginPage";
import RegisterPage from "./pages/Register/RegisterPage";
import ProtectedRoute from "./components/auth/ProtectedRoute";
import { useAuth } from "./components/auth/AuthProvider";
import { getCategories, type Category } from "./api/categoriesApi";
import ProductDetailsPage from "./pages/ProductDetails/ProductDetailsPage";
import CartPage from "./pages/Cart/CartPage";
import UserAddressesPage from "./pages/Profile/UserAddressesPage";
import MyOrdersPage from "./pages/Profile/MyOrdersPage";
import OrderDetailsPage from "./pages/Profile/OrderDetailsPage";
import { NotificationProvider } from "./context/NotificationContext";
import NotificationToast from "./components/NotificationToast/NotificationToast";
import ScrollToTopButton from "./components/ScrollToTopButton/ScrollToTopButton";

function AppContent() {
  const { isAuthenticated } = useAuth();
  const [categories, setCategories] = useState<Category[]>([]);

  useEffect(() => {
    async function loadCategories() {
      try {
        const data = await getCategories();
        setCategories(Array.isArray(data) ? data : []);
      } catch (error) {
        console.error("Не удалось загрузить категории для меню", error);
        setCategories([]);
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
              <span className="nav-dropdown__arrow"></span>
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

          {isAuthenticated && (
            <Link to="/cart" className="nav-link">
              Корзина
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

      <main className="main-content">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/categories" element={<CategoriesPage />} />
          <Route path="/products" element={<ProductsPage />} />
          <Route path="/products/:id" element={<ProductDetailsPage />} />
          <Route path="/about" element={<AboutPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />

          <Route
            path="/profile"
            element={
              <ProtectedRoute>
                <ProfilePage />
              </ProtectedRoute>
            }
          />

          <Route
            path="/profile/addresses"
            element={
              <ProtectedRoute>
                <UserAddressesPage />
              </ProtectedRoute>
            }
          />

          <Route
            path="/profile/orders"
            element={
              <ProtectedRoute>
                <MyOrdersPage />
              </ProtectedRoute>
            }
          />

          <Route
            path="/profile/orders/:id"
            element={
              <ProtectedRoute>
                <OrderDetailsPage />
              </ProtectedRoute>
            }
          />

          <Route
            path="/cart"
            element={
              <ProtectedRoute>
                <CartPage />
              </ProtectedRoute>
            }
          />

          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </main>

      <NotificationToast />
      <ScrollToTopButton offset={400} />
    </div>
  );
}

function App() {
  return (
    <BrowserRouter>
      <NotificationProvider>
        <AppContent />
      </NotificationProvider>
    </BrowserRouter>
  );
}

export default App;