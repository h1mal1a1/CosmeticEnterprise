export type FinishedProductImage = {
    id: number;
    fileUrl: string;
    sortOrder: number;
    isMain: boolean;
}

export type FinishedProduct = {
    id: number;
    name: string;
    idRecipe: number;
    idProductCategory: number;
    idUnitsOfMeasurement: number;
    images: FinishedProductImage[];
};

export type CreateFinishedProductRequest = {
    name: string;
    idRecipe: number;
    idProductCategory: number;
    idUnitsOfMeasurement: number;
};


export type UpdateFinishedProductRequest = {
    name: string;
    idRecipe: number;
    idProductCategory: number;
    idUnitsOfMeasurement: number;
};

