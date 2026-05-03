import { Admin, Resource } from "react-admin";
import { dataProvider } from "../api/dataProvider";
import { authProvider } from "../auth/authProvider";

import {
  CategoryCreate,
  CategoryEdit,
  CategoryList,
} from "../resources/categories";

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
      <p>Управление справочниками</p>
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
    </Admin>
  );
}

export default App;