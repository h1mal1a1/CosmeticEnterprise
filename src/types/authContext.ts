import type { CurrentUser } from "./auth";

export type AuthContextValue = {
    token: string | null;
    user: CurrentUser | null;
    isAuthenticated: boolean;
    isLoading: boolean;
    login: (token: string) => Promise<void>;
    logout: () => void;
};