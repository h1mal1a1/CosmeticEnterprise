export type FinishedProduct = {
    id: number;
    name: string;
    idRecipe: number;
    idProductCategory: number;
    idUnitsOfMeasurement: number;
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

