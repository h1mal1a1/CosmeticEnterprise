import { Link, useLocation } from 'react-router-dom';
import './AppBreadcrumbs.css';

type BreadcrumbItem = {
  label: string;
  path: string;
};

const routeLabels: Record<string, string> = {
  categories: 'Категории',
  products: 'Продукция',
  about: 'О нас',
  login: 'Вход',
  profile: 'Личный кабинет',
  admin: 'Панель управления',
};

function isNumericSegment(value: string): boolean {
  return /^\d+$/.test(value);
}

function buildBreadcrumbs(pathname: string): BreadcrumbItem[] {
  const segments = pathname.split('/').filter(Boolean);

  const breadcrumbs: BreadcrumbItem[] = [
    { label: 'Главная', path: '/' },
  ];

  let currentPath = '';

  for (let index = 0; index < segments.length; index++) {
    const segment = segments[index];
    currentPath += `/${segment}`;

    let label = routeLabels[segment] ?? decodeURIComponent(segment);

    if (segments[index - 1] === 'products' && isNumericSegment(segment)) {
      label = 'Карточка товара';
    }

    breadcrumbs.push({
      label,
      path: currentPath,
    });
  }

  return breadcrumbs;
}

export default function AppBreadcrumbs() {
  const location = useLocation();

  if (location.pathname === '/') {
    return null;
  }

  const breadcrumbs = buildBreadcrumbs(location.pathname);

  return (
    <nav className="app-breadcrumbs" aria-label="breadcrumb">
      <ol className="app-breadcrumbs__list">
        {breadcrumbs.map((item, index) => {
          const isLast = index === breadcrumbs.length - 1;

          return (
            <li
              key={item.path}
              className={`app-breadcrumbs__item ${isLast ? 'app-breadcrumbs__item--current' : ''}`}
            >
              {isLast ? (
                <span>{item.label}</span>
              ) : (
                <Link to={item.path} className="app-breadcrumbs__link">
                  {item.label}
                </Link>
              )}

              {!isLast && (
                <span className="app-breadcrumbs__separator" aria-hidden="true">
                  /
                </span>
              )}
            </li>
          );
        })}
      </ol>
    </nav>
  );
}