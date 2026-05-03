import { Datagrid, EditButton, List, TextField } from "react-admin";

export const CategoryList = () => (
  <List pagination={false}>
    <Datagrid rowClick="edit">
      <TextField source="id" label="ID" />
      <TextField source="name" label="Название" />
      <EditButton label="Редактировать" />
    </Datagrid>
  </List>
);