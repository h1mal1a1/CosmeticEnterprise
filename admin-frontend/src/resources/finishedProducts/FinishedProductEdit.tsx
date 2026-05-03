import {
  Edit,
  NumberInput,
  ReferenceInput,
  required,
  SelectInput,
  SimpleForm,
  TextInput,
} from "react-admin";

const emptyToNull = (value: string | null | undefined) => {
  const trimmedValue = value?.trim();

  return trimmedValue ? trimmedValue : null;
};

export const FinishedProductEdit = () => (
  <Edit>
    <SimpleForm>
      <TextInput source="id" label="ID" disabled />

      <TextInput source="name" label="Название" validate={required()} />

      <NumberInput source="price" label="Цена" validate={required()} />

      <TextInput
        source="wbUrl"
        label="Ссылка на Wildberries"
        parse={emptyToNull}
        fullWidth
      />

      <ReferenceInput
        source="idProductCategory"
        reference="product-categories"
      >
        <SelectInput label="Категория" optionText="name" validate={required()} />
      </ReferenceInput>

      <ReferenceInput source="idRecipe" reference="recipes">
        <SelectInput label="Рецептура" optionText="name" validate={required()} />
      </ReferenceInput>

      <ReferenceInput
        source="idUnitsOfMeasurement"
        reference="units-of-measurement"
      >
        <SelectInput
          label="Единица измерения"
          optionText="name"
          validate={required()}
        />
      </ReferenceInput>
    </SimpleForm>
  </Edit>
);