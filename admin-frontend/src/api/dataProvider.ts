import type { DataProvider } from "react-admin";

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