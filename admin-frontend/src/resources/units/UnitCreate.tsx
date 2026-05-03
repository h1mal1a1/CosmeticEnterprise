import { Create, required, SimpleForm, TextInput } from "react-admin";

export const UnitCreate = () => (
  <Create>
    <SimpleForm>
      <TextInput source="name" label="Название" validate={required()} />
    </SimpleForm>
  </Create>
);