import {
  Create,
  minValue,
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

export const FinishedProductCreate = () => (
  <Create>
    <SimpleForm defaultValues={{ availableQuantity: 0 }}>
      <TextInput source="name" label="Название" validate={required()} />

      <NumberInput source="price" label="Цена" validate={required()} />

      <NumberInput
        source="availableQuantity"
        label="Количество"
        min={0}
        validate={[required(), minValue(0)]}
      />

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
  </Create>
);