const API_URL = "/api";

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
};

class ApiError extends Error {
  status: number;

  constructor(message: string, status: number) {
    super(message);
    this.status = status;
  }
}

async function request<T>(url: string, options: RequestInit = {}): Promise<T> {
  const response = await fetch(`${API_URL}${url}`, {
    ...options,
    credentials: "include",
  });

  if (!response.ok) {
    throw new ApiError(`Ошибка запроса: ${response.status}`, response.status);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json();
}

export function getFinishedProductById(id: number): Promise<FinishedProduct> {
  return request<FinishedProduct>(`/finished-products/${id}`);
}

export function uploadFinishedProductImages(
  finishedProductId: number,
  files: File[],
): Promise<FinishedProductImage[]> {
  const formData = new FormData();

  files.forEach((file) => {
    formData.append("files", file);
  });

  return request<FinishedProductImage[]>(
    `/finished-products/${finishedProductId}/images`,
    {
      method: "POST",
      body: formData,
    },
  );
}

export function deleteFinishedProductImage(
  finishedProductId: number,
  imageId: number,
): Promise<void> {
  return request<void>(
    `/finished-products/${finishedProductId}/images/${imageId}`,
    {
      method: "DELETE",
    },
  );
}

export function setMainFinishedProductImage(
  finishedProductId: number,
  imageId: number,
): Promise<FinishedProductImage[]> {
  return request<FinishedProductImage[]>(
    `/finished-products/${finishedProductId}/images/${imageId}/set-main`,
    {
      method: "PUT",
    },
  );
}