import { Admin, Resource } from "react-admin";
import { dataProvider } from "../api/dataProvider";
import { authProvider } from "../auth/authProvider";
import {
  CategoryCreate,
  CategoryEdit,
  CategoryList,
} from "../resources/categories";

function Dashboard() {
  return (
    <div style={{ padding: 24 }}>
      <h1>Админка CosmeticEnterprise</h1>
      <p>Структура выстроена правильно. Первый ресурс — категории.</p>
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
    </Admin>
  );
}

export default App;