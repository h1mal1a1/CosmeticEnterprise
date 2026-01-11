/*Мой контент*/
import { BrowserRouter,Routes, Route, Link } from 'react-router-dom';
import './App.css'
import HomePage from './pages/HomePage';
import CategoriesPage from './pages/CategoriesPage';
import ProductsPage from './pages/ProductsPage';
import ProfilePage from './pages/ProfilePage';
import ReviewsPage from './pages/ReviewsPage';
import AboutPage from './pages/AboutPage';
import ContactsPage from './pages/ContactsPage';
import UploadPage from './pages/UploadPage'

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
          <Link to="/upload" className='nav-link'>Загрузить файлы</Link>
        </nav>
        <main className="main-content"/>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/categories" element={<CategoriesPage />} />
          <Route path="/products" element={<ProductsPage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/reviews" element={<ReviewsPage />} />
          <Route path="/about" element={<AboutPage />} />
          <Route path="/contacts" element={<ContactsPage />} />
          <Route path="/upload" element={<UploadPage />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App