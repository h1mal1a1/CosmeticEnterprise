import {
  Datagrid,
  EditButton,
  List,
  NumberField,
  ReferenceField,
  TextField,
  UrlField,
} from "react-admin";

export const FinishedProductList = () => (
  <List pagination={false}>
    <Datagrid rowClick="edit">
      <TextField source="id" label="ID" />
      <TextField source="name" label="Название" />
      <NumberField source="price" label="Цена" />
      <UrlField source="wbUrl" label="Wildberries" />
      <ReferenceField
        source="idProductCategory"
        reference="product-categories"
        label="Категория"
      >
        <TextField source="name" />
      </ReferenceField>
      <ReferenceField source="idRecipe" reference="recipes" label="Рецептура">
        <TextField source="name" />
      </ReferenceField>
      <ReferenceField
        source="idUnitsOfMeasurement"
        reference="units-of-measurement"
        label="Ед. изм."
      >
        <TextField source="name" />
      </ReferenceField>
      <NumberField source="availableQuantity" label="Остаток" />
      <EditButton label="Редактировать" />
    </Datagrid>
  </List>
);