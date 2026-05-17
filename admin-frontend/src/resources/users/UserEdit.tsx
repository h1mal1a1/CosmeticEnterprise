import {
  BooleanInput,
  Edit,
  required,
  SelectInput,
  SimpleForm,
  TextInput,
} from "react-admin";

const roleChoices = [
  { id: 0, name: "Пользователь" },
  { id: 1, name: "Менеджер" },
  { id: 2, name: "Кладовщик" },
  { id: 3, name: "Администратор" },
];

type UserFormData = {
  username?: string;
  email?: string;
  phone?: string;
  roleName?: number | string;
  isActive?: boolean;
};

const transformUserData = (data: UserFormData) => {
  return {
    username: data.username?.trim() ?? "",
    email: data.email?.trim() ?? "",
    phone: data.phone?.trim() ?? "",
    roleName: Number(data.roleName),
    isActive: Boolean(data.isActive),
  };
};

export const UserEdit = () => (
  <Edit transform={transformUserData} mutationMode="pessimistic">
    <SimpleForm>
      <TextInput source="id" label="ID" disabled />

      <TextInput source="username" label="Логин" validate={required()} />

      <TextInput source="email" label="Почта" validate={required()} />

      <TextInput source="phone" label="Телефон" validate={required()} />

      <SelectInput
        source="roleName"
        label="Роль"
        choices={roleChoices}
        validate={required()}
      />

      <BooleanInput source="isActive" label="Активен" />
    </SimpleForm>
  </Edit>
);