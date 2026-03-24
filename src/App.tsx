import { BrowserRouter, Routes, Route, Link, Navigate } from 'react-router-dom';
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

function AppContent() {
  const { isAuthenticated, user } = useAuth();

  const isAdmin = user?.roleName?.toLowerCase() === 'admin';

  return (
    <div className="page-container">
      <nav className="top-nav">
        <div className="brand-title">VALMÉRIS</div>

        <div className="nav-links">
          <Link to="/" className="nav-link">Главная</Link>
          <Link to="/categories" className="nav-link">Категории</Link>
          <Link to="/products" className="nav-link">Продукция</Link>
          <Link to="/about" className="nav-link">О нас</Link>

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

      <main className="main-content">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/categories" element={<CategoriesPage />} />
          <Route path="/products" element={<ProductsPage />} />
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