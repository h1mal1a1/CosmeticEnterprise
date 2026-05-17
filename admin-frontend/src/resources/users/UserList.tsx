import {
  BooleanField,
  Datagrid,
  EditButton,
  FunctionField,
  List,
  TextField,
} from "react-admin";

type UserRecord = {
  roleName: number | string;
};

const getRoleLabel = (roleName: number | string) => {
  switch (roleName) {
    case 0:
    case "0":
    case "User":
      return "Пользователь";

    case 1:
    case "1":
    case "Manager":
      return "Менеджер";

    case 2:
    case "2":
    case "WarehouseManager":
      return "Кладовщик";

    case 3:
    case "3":
    case "Admin":
      return "Администратор";

    default:
      return String(roleName);
  }
};

export const UserList = () => (
  <List pagination={false}>
    <Datagrid rowClick="edit">
      <TextField source="id" label="ID" />
      <TextField source="username" label="Логин" />
      <TextField source="email" label="Почта" />
      <TextField source="phone" label="Телефон" />

      <FunctionField<UserRecord>
        source="roleName"
        label="Роль"
        render={(record) => getRoleLabel(record.roleName)}
      />

      <BooleanField source="isActive" label="Активен" />
      <EditButton label="Редактировать" />
    </Datagrid>
  </List>
);