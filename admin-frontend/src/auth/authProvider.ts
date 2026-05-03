import type { AuthProvider } from "react-admin";

const API_URL = "http://localhost:8080/api";

const allowedAdminRoles = ["Admin"];

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
  },

  logout: async () => {
    await fetch(`${API_URL}/auth/logout`, {
      method: "POST",
      credentials: "include",
    });
  },

  checkAuth: async () => {
    const response = await fetch(`${API_URL}/auth/me`, {
      credentials: "include",
    });

    if (!response.ok) {
      throw new Error("Не авторизован");
    }

    const user = await response.json();

    if (!allowedAdminRoles.includes(user.roleName)) {
      throw new Error("Нет доступа к админке");
    }
  },

  checkError: async (error) => {
    if (error.status === 401 || error.status === 403) {
      throw new Error("Ошибка авторизации");
    }
  },

  getPermissions: async () => {
    const response = await fetch(`${API_URL}/auth/me`, {
      credentials: "include",
    });

    if (!response.ok) {
      return null;
    }

    const user = await response.json();

    return user.roleName;
  },

  getIdentity: async () => {
    const response = await fetch(`${API_URL}/auth/me`, {
      credentials: "include",
    });

    if (!response.ok) {
      throw new Error("Не удалось получить пользователя");
    }

    const user = await response.json();

    return {
      id: user.idUser,
      fullName: user.username,
    };
  },
};