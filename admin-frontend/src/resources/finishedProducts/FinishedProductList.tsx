import {
  Button,
  Datagrid,
  EditButton,
  FunctionField,
  List,
  NumberField,
  ReferenceField,
  TextField,
  UrlField,
} from "react-admin";
import { Link } from "react-router-dom";

type FinishedProductRecord = {
  id: number;
};

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
      <FunctionField<FinishedProductRecord>
        label="Изображения"
        render={(record) => (
          <Button
            component={Link}
            to={`/finished-products/${record.id}/images`}
            label="Изображения"
            onClick={(event) => event.stopPropagation()}
          />
        )}
      />
      <EditButton label="Редактировать" />
    </Datagrid>
  </List>
);