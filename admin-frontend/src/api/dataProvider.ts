import type { DataProvider } from "react-admin";

const API_URL = "/api";

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
    throw new Error(`Ошибка запроса: ${response.status}`);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json();
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
    const data = await request<any>(`/${resource}`, {
      method: "POST",
      body: JSON.stringify(params.data),
    });

    return { data };
  },

  update: async (resource, params) => {
    const data = await request<any>(`/${resource}/${params.id}`, {
      method: "PUT",
      body: JSON.stringify(params.data),
    });

    return { data };
  },

  delete: async (resource, params) => {
    await request<void>(`/${resource}/${params.id}`, {
      method: "DELETE",
    });

    return {
      data: (params.previousData ?? { id: params.id }) as any,
    };
  },

  getMany: async () => {
    throw new Error("getMany не реализован");
  },

  getManyReference: async () => {
    throw new Error("getManyReference не реализован");
  },

  updateMany: async () => {
    throw new Error("updateMany не реализован");
  },

  deleteMany: async () => {
    throw new Error("deleteMany не реализован");
  },
};