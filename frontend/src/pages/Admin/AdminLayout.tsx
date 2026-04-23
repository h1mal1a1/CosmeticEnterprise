import { NavLink, Outlet } from "react-router-dom";
import "./AdminLayout.css";

export default function AdminLayout() {
  return (
    <div className="admin-layout-root">
      <aside className="admin-sidebar">
        <div className="admin-sidebar__title">Управление</div>

        <nav className="admin-sidebar__nav">
          <NavLink
            to="/admin/categories"
            className={({ isActive }) =>
              `admin-sidebar__link ${isActive ? "admin-sidebar__link--active" : ""}`
            }
          >
            Категории
          </NavLink>

          <NavLink
            to="/admin/recipes"
            className={({ isActive }) =>
              `admin-sidebar__link ${isActive ? "admin-sidebar__link--active" : ""}`
            }
          >
            Рецептуры
          </NavLink>

          <NavLink
            to="/admin/units-of-measurement"
            className={({ isActive }) =>
              `admin-sidebar__link ${isActive ? "admin-sidebar__link--active" : ""}`
            }
          >
            Единицы измерения
          </NavLink>

          <NavLink
            to="/admin/finished-products"
            className={({ isActive }) =>
              `admin-sidebar__link ${isActive ? "admin-sidebar__link--active" : ""}`
            }
          >
            Готовая продукция
          </NavLink>

          <NavLink
            to="/admin/orders"
            className={({ isActive }) =>
              `admin-sidebar__link ${isActive ? "admin-sidebar__link--active" : ""}`
            }
          >
            Заказы
          </NavLink>
        </nav>
      </aside>

      <section className="admin-content">
        <Outlet />
      </section>
    </div>
  );
}