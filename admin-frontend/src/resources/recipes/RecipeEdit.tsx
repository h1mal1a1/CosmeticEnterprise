import { Edit, required, SimpleForm, TextInput } from "react-admin";

export const RecipeEdit = () => (
  <Edit>
    <SimpleForm>
      <TextInput source="id" label="ID" disabled />
      <TextInput source="name" label="Название" validate={required()} />
    </SimpleForm>
  </Edit>
);