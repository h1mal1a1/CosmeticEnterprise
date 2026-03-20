export type LoginRequest = {
    username: string;
    password: string;
};

export type AuthResponse = {
    accessToken: string;
    refreshToken?: string;
}

export type CurrentUser = {
    idUser: number;
    username: string;
    roleName: string;
};