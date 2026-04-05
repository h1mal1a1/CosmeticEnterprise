import { Link, useLocation, useParams } from 'react-router-dom';
import './AppBreadcrumbs.css';

const routeLabels: Record<string, string> = {
  categories: 'Категории',
  products: 'Продукция',
  about: 'О нас',
  profile: 'Личный кабинет',
  login: 'Вход',
  admin: 'Панель управления',
  recipes: 'Рецептуры',
  'units-of-measurement': 'Единицы измерения',
  'finished-products': 'Готовая продукция',
  images: 'Изображения',
};

function isNumericSegment(value: string) {
  return /^\d+$/.test(value);
}

export default function AppBreadcrumbs() {
  const location = useLocation();
  const params = useParams();

  const pathnames = location.pathname.split('/').filter(Boolean);

  if (pathnames.length === 0) {
    return null;
  }

  return (
    <div className="breadcrumbs">
      <Link to="/" className="breadcrumbs__link">
        Главная
      </Link>

      {pathnames.map((segment, index) => {
        const to = `/${pathnames.slice(0, index + 1).join('/')}`;
        const isLast = index === pathnames.length - 1;

        if (isNumericSegment(segment)) {
          return null;
        }

        let label = routeLabels[segment] ?? segment;

        if (segment === 'products' && params.id) {
          label = 'Продукция';
        }

        return (
          <span key={to} className="breadcrumbs__item">
            <span className="breadcrumbs__separator">/</span>
            {isLast ? (
              <span className="breadcrumbs__current">{label}</span>
            ) : (
              <Link to={to} className="breadcrumbs__link">
                {label}
              </Link>
            )}
          </span>
        );
      })}
    </div>
  );
}