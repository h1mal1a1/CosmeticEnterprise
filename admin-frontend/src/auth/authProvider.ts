import type { AuthProvider } from "react-admin";

const API_URL = "/api";

const allowedAdminRoles = ["admin"];

type CurrentUser = {
  idUser: number;
  username: string;
  email: string;
  phone: string;
  roleName: string;
};

async function getCurrentUser(): Promise<CurrentUser> {
  const response = await fetch(`${API_URL}/auth/me`, {
    credentials: "include",
  });

  if (!response.ok) {
    throw new Error("Не авторизован");
  }

  return response.json();
}

function isAdmin(user: CurrentUser): boolean {
  return allowedAdminRoles.includes(user.roleName.toLowerCase());
}

export const authProvider: AuthProvider = {
  login: async ({ username, password }) => {
    const response = await fetch(`${API_URL}/auth/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "include",
      body: JSON.stringify({
        username,
        password,
      }),
    });

    if (!response.ok) {
      throw new Error("Неверный логин или пароль");
    }

    const user = await getCurrentUser();

    if (!isAdmin(user)) {
      await fetch(`${API_URL}/auth/logout`, {
        method: "POST",
        credentials: "include",
      });

      throw new Error("Нет доступа к админке");
    }
  },

  logout: async () => {
    await fetch(`${API_URL}/auth/logout`, {
      method: "POST",
      credentials: "include",
    });
  },

  checkAuth: async () => {
    const user = await getCurrentUser();

    if (!isAdmin(user)) {
      throw new Error("Нет доступа к админке");
    }
  },

  checkError: async (error) => {
    if (error?.status === 401 || error?.status === 403) {
      throw new Error("Ошибка авторизации");
    }
  },

  getPermissions: async () => {
    const user = await getCurrentUser();

    return user.roleName;
  },

  getIdentity: async () => {
    const user = await getCurrentUser();

    return {
      id: user.idUser,
      fullName: user.username,
    };
  },
};