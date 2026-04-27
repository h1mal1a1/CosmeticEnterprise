export type LoginRequest = {
  username: string;
  password: string;
};

export type RegisterRequest = {
  username: string;
  password: string;
  email: string;
  phone: string;
};

export type UpdateProfileRequest = {
  email: string;
  phone: string;
};

export type AuthResponse = {
  accessToken: string;
  refreshToken?: string;
};

export type CurrentUser = {
  idUser: number;
  username: string;
  email: string;
  phone: string;
  roleName: string;
};