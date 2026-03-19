export type LoginRequest = {
    username: string;
    password: string;
};

export type AuthResponse = {
    accessToken: string;
    refreshToken?: string;
}

export type CurrentUser = {
    id: number;
    username: string;
    role: string;
};