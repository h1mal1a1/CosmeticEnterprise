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
    wbUrl?: string | null;
    idRecipe: number;
    idProductCategory: number;
    idUnitsOfMeasurement: number;
    availableQuantity: number;
    images: FinishedProductImage[];
    description?: string | null;
};

export type CreateFinishedProductRequest = {
    name: string;
    price: number;
    wbUrl?: string | null;
    idRecipe: number;
    idProductCategory: number;
    idUnitsOfMeasurement: number;
    description?: string | null;
};

export type UpdateFinishedProductRequest = {
    name: string;
    price: number;
    wbUrl?: string | null;
    idRecipe: number;
    idProductCategory: number;
    idUnitsOfMeasurement: number;
    description?: string | null;
};