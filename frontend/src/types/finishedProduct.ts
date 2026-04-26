export type FinishedProductImage = {
    id: number;
    fileUrl: string;
    sortOrder: number;
    isMain: boolean;
};

export type FinishedProduct = {
    id: number;
    name: string;
    price: number;
    idRecipe: number;
    idProductCategory: number;
    idUnitsOfMeasurement: number;
    availableQuantity: number;
    images: FinishedProductImage[];
};

export type CreateFinishedProductRequest = {
    name: string;
    price: number;
    idRecipe: number;
    idProductCategory: number;
    idUnitsOfMeasurement: number;
};

export type UpdateFinishedProductRequest = {
    name: string;
    price: number;
    idRecipe: number;
    idProductCategory: number;
    idUnitsOfMeasurement: number;
};