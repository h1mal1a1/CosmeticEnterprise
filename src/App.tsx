import { BrowserRouter,Routes, Route, Link } from 'react-router-dom';
import './App.css'
import HomePage from './pages/Home/HomePage';
import CategoriesPage from './pages/Categories/CategoriesPage';
import ProductsPage from './pages/Products/ProductsPage';
import ProfilePage from './pages/Profile/ProfilePage';
import ReviewsPage from './pages/Reviews/ReviewsPage';
import AboutPage from './pages/About/AboutPage';
import ContactsPage from './pages/Contacts/ContactsPage';
import UploadPage from './pages/Upload/UploadPage'
import AdminPage from './pages/Admin/AdminPage';
import LoginPage from './pages/Login/LoginPage';
import ProtectedRoute from './components/auth/ProtectedRoute';

function App() {
  return (
    <BrowserRouter>
      <div className="page-container">
        <nav className='top-nav'> 
          <div className='brand-title'>VALMÉRIS</div>
          <Link to="/" className='nav-link'>Главная</Link>
          <Link to="/categories" className='nav-link'>Категории</Link>
          <Link to="/products" className='nav-link'>Продукция</Link>
          <Link to="/profile" className='nav-link'>Личный кабинет</Link>
          <Link to="/reviews" className='nav-link'>Отзывы</Link>
          <Link to="/about" className='nav-link'>О нас</Link>
          <Link to="/contacts" className='nav-link'>Контакты</Link>
          <Link to="/admin" className='nav-link'>Админка</Link>
          <Link to="/upload" className='nav-link'>Загрузить файлы</Link>
          <Link to="/login" className='nav-link'>Войти</Link>
        </nav>
        <main className="main-content">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/categories" element={<CategoriesPage />} />
            <Route path="/products" element={<ProductsPage />} />
            <Route path="/profile" element={<ProfilePage />} />
            <Route path="/reviews" element={<ReviewsPage />} />
            <Route path="/about" element={<AboutPage />} />
            <Route path="/contacts" element={<ContactsPage />} />
            <Route path="/admin" element={<AdminPage />} />
            <Route path="/upload" element={<UploadPage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route 
              path="/admin" 
              element={
                <ProtectedRoute>
                  <AdminPage />
                </ProtectedRoute>
              } 
            />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

export default App