export type FinishedProduct = {
    id_finished_product: number;
    name: string;
    description?: string;
    price?: number;
};

export type CreateFinishedProductRequest = {
    name: string;
    description?: string;
    price?: number;
};

export type UpdateFinishedProductRequest = {
    name?: string;
    description?: string;
    price?: number;
};

