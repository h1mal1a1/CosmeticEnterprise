import type { DataProvider, Identifier } from "react-admin";

const API_URL = "/api";

class HttpError extends Error {
  status: number;

  constructor(message: string, status: number) {
    super(message);
    this.status = status;
  }
}

const request = async <T>(url: string, options: RequestInit = {}): Promise<T> => {
  const response = await fetch(`${API_URL}${url}`, {
    ...options,
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      ...(options.headers ?? {}),
    },
  });

  if (!response.ok) {
    throw new HttpError(`Ошибка запроса: ${response.status}`, response.status);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json();
};

const hasStockValue = (value: unknown) => {
  return value !== undefined && value !== null && value !== "";
};

const normalizeUrl = (value: unknown) => {
  if (value === undefined || value === null) {
    return null;
  }

  if (typeof value !== "string") {
    return value;
  }

  const trimmedValue = value.trim();

  if (!trimmedValue) {
    return null;
  }

  const hasProtocol = /^(https?|ftp):\/\//i.test(trimmedValue);

  return hasProtocol ? trimmedValue : `https://${trimmedValue}`;
};

const updateFinishedProductStock = async (
  id: Identifier,
  availableQuantity: unknown,
) => {
  if (!hasStockValue(availableQuantity)) {
    return null;
  }

  return await request<{
    id: number;
    quantity: number;
    reservedQuantity: number;
    availableQuantity: number;
  }>(`/finished-products/${id}/stock`, {
    method: "PUT",
    body: JSON.stringify({
      availableQuantity: Number(availableQuantity),
    }),
  });
};

const buildFinishedProductRequest = (data: Record<string, unknown>) => {
  const {
    id: _id,
    images: _images,
    availableQuantity: _availableQuantity,
    ...requestData
  } = data;

  return {
    ...requestData,
    wbUrl: normalizeUrl(requestData.wbUrl),
  };
};

export const dataProvider: DataProvider = {
  getList: async (resource) => {
    const data = await request<any[]>(`/${resource}`);

    return {
      data,
      total: data.length,
    };
  },

  getOne: async (resource, params) => {
    const data = await request<any>(`/${resource}/${params.id}`);

    return { data };
  },

  create: async (resource, params) => {
    if (resource === "finished-products") {
      const productData = buildFinishedProductRequest(params.data);

      const createdProduct = await request<any>(`/${resource}`, {
        method: "POST",
        body: JSON.stringify(productData),
      });

      const stock = await updateFinishedProductStock(
        createdProduct.id,
        params.data.availableQuantity,
      );

      return {
        data: {
          ...createdProduct,
          ...(stock
            ? {
                availableQuantity: stock.availableQuantity,
              }
            : {}),
        },
      };
    }

    const data = await request<any>(`/${resource}`, {
      method: "POST",
      body: JSON.stringify(params.data),
    });

    return { data };
  },

  update: async (resource, params) => {
    if (resource === "finished-products") {
      const productData = buildFinishedProductRequest(params.data);

      const updatedProduct = await request<any | undefined>(
        `/${resource}/${params.id}`,
        {
          method: "PUT",
          body: JSON.stringify(productData),
        },
      );

      const stock = await updateFinishedProductStock(
        params.id,
        params.data.availableQuantity,
      );

      return {
        data: {
          ...(updatedProduct ?? {
            ...params.previousData,
            ...productData,
            id: params.id,
          }),
          ...(stock
            ? {
                availableQuantity: stock.availableQuantity,
              }
            : {}),
        },
      };
    }

    const data = await request<any | undefined>(`/${resource}/${params.id}`, {
      method: "PUT",
      body: JSON.stringify(params.data),
    });

    return {
      data: data ?? {
        ...params.previousData,
        ...params.data,
        id: params.id,
      },
    };
  },

  delete: async (resource, params) => {
    await request<void>(`/${resource}/${params.id}`, {
      method: "DELETE",
    });

    return {
      data: (params.previousData ?? { id: params.id }) as any,
    };
  },

  getMany: async (resource, params) => {
    const data = await Promise.all(
      params.ids.map((id) => request<any>(`/${resource}/${id}`)),
    );

    return { data };
  },

  getManyReference: async () => {
    throw new Error("getManyReference не реализован");
  },

  updateMany: async (resource, params) => {
    await Promise.all(
      params.ids.map((id) =>
        request<void>(`/${resource}/${id}`, {
          method: "PUT",
          body: JSON.stringify(params.data),
        }),
      ),
    );

    return { data: params.ids };
  },

  deleteMany: async (resource, params) => {
    await Promise.all(
      params.ids.map((id) =>
        request<void>(`/${resource}/${id}`, {
          method: "DELETE",
        }),
      ),
    );

    return { data: params.ids };
  },
};