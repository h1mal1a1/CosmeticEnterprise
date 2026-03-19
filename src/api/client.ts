import { ApiError } from "../types/api";
import { env } from '../config/env';

type HttpMethod = 'GET' | 'POST' | 'PUT' | 'PATCH' |'DELETE';

type ApiRequestOptions = {
  method?: HttpMethod;
  body?: unknown;
  headers?: Record<string, string>;
  signal?: AbortSignal;
  token?: string | null;
};

export async function apiRequest<TResponse>(
    path: string,
    options: ApiRequestOptions = {}
): Promise<TResponse> {
    const {
        method = 'GET',
        body,
        headers = {},
        signal,
        token,
    } = options;

    const requestHeaders: Record<string, string> = {
        ...headers,
    };
    
    if(body !== undefined) {
        requestHeaders['Content-Type'] = 'application/json';
    }

    if(token) {
        requestHeaders['Authorization'] = `Bearer ${token}`;
    }

    const response = await fetch(`${env.apiBaseUrl}${path}`, {
        method,
        headers: requestHeaders,
        body: body !== undefined ? JSON.stringify(body) : undefined,
        signal,
    });

    const contentType = response.headers.get('content-type') ?? '';
    const isJson = contentType.includes('application/json');

    let responseData: unknown = null;
    if(response.status !== 204) {
        responseData = isJson 
        ? await response.json() 
        : await response.text();
    }

    if(!response.ok) {
        let errorMessage = 'Ошибка при выполнении запроса';
        if(typeof responseData === 'string' && responseData.trim()) {
            errorMessage = responseData;
        }

        if (responseData && typeof responseData === 'object') {
            if ('message' in responseData && typeof responseData.message === 'string') {
                errorMessage = responseData.message;
            } else if ('detail' in responseData && typeof responseData.detail === 'string') {
                errorMessage = responseData.detail;
            } else if ('title' in responseData && typeof responseData.title === 'string') {
                errorMessage = responseData.title;
            }
        }

        throw new ApiError(response.status, errorMessage);
    }

    return responseData as TResponse;
}