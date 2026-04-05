import { apiRequest } from "./client";

export type Recipe = {
  id: number;
  name: string;
};

export type CreateRecipeRequest = {
  name: string;
};

export type UpdateRecipeRequest = {
  name: string;
};

export function getRecipes(): Promise<Recipe[]> {
  return apiRequest<Recipe[]>("/api/recipes");
}

export function createRecipe(request: CreateRecipeRequest): Promise<Recipe> {
  return apiRequest<Recipe>("/api/recipes", {
    method: "POST",
    body: request,
  });
}

export function updateRecipe(
  id: number,
  request: UpdateRecipeRequest,
): Promise<void> {
  return apiRequest<void>(`/api/recipes/${id}`, {
    method: "PUT",
    body: request,
  });
}

export function deleteRecipe(id: number): Promise<void> {
  return apiRequest<void>(`/api/recipes/${id}`, {
    method: "DELETE",
  });
}