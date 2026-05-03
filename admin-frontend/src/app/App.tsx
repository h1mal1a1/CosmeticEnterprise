import { Admin, CustomRoutes, Resource } from "react-admin";
import { Route } from "react-router-dom";
import { dataProvider } from "../api/dataProvider";
import { authProvider } from "../auth/authProvider";

import FinishedProductImagesPage from "../pages/FinishedProductImages/FinishedProductImagesPage";
import OrderDetailsPage from "../pages/OrderDetails/OrderDetailsPage";
import OrdersPage from "../pages/Orders/OrdersPage";

import {
  CategoryCreate,
  CategoryEdit,
  CategoryList,
} from "../resources/categories";

import {
  FinishedProductCreate,
  FinishedProductEdit,
  FinishedProductList,
} from "../resources/finishedProducts";

import {
  RecipeCreate,
  RecipeEdit,
  RecipeList,
} from "../resources/recipes";

import {
  UnitCreate,
  UnitEdit,
  UnitList,
} from "../resources/units";

function Dashboard() {
  return (
    <div style={{ padding: 24 }}>
      <h1>Админка CosmeticEnterprise</h1>
      <p>Управление справочниками, продукцией, изображениями и заказами.</p>
    </div>
  );
}

function App() {
  return (
    <Admin
      dataProvider={dataProvider}
      authProvider={authProvider}
      dashboard={Dashboard}
      requireAuth
    >
      <Resource
        name="product-categories"
        options={{ label: "Категории" }}
        list={CategoryList}
        edit={CategoryEdit}
        create={CategoryCreate}
      />

      <Resource
        name="recipes"
        options={{ label: "Рецептуры" }}
        list={RecipeList}
        edit={RecipeEdit}
        create={RecipeCreate}
      />

      <Resource
        name="units-of-measurement"
        options={{ label: "Единицы измерения" }}
        list={UnitList}
        edit={UnitEdit}
        create={UnitCreate}
      />

      <Resource
        name="finished-products"
        options={{ label: "Готовая продукция" }}
        list={FinishedProductList}
        edit={FinishedProductEdit}
        create={FinishedProductCreate}
      />

      <Resource name="orders" options={{ label: "Заказы" }} list={OrdersPage} />

      <CustomRoutes>
        <Route
          path="/finished-products/:id/images"
          element={<FinishedProductImagesPage />}
        />

        <Route path="/orders/:id" element={<OrderDetailsPage />} />
      </CustomRoutes>
    </Admin>
  );
}

export default App;