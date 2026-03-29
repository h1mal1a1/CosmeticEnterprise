import { env } from '../config/env';
import type { FinishedProductImage } from '../types/finishedProduct';

export async function uploadFinishedProductImage(
  finishedProductId: number,
  file: File,
): Promise<FinishedProductImage> {
  const formData = new FormData();
  formData.append('file', file);

  const response = await fetch(
    `${env.apiBaseUrl}/api/finished-products/${finishedProductId}/images`,
    {
      method: 'POST',
      body: formData,
      credentials: 'include',
    },
  );

  if (!response.ok) {
    throw new Error(`Ошибка загрузки изображения: ${response.status}`);
  }

  return response.json() as Promise<FinishedProductImage>;
}

export async function deleteFinishedProductImage(
  finishedProductId: number,
  imageId: number,
): Promise<void> {
  const response = await fetch(
    `${env.apiBaseUrl}/api/finished-products/${finishedProductId}/images/${imageId}`,
    {
      method: 'DELETE',
      credentials: 'include',
    },
  );

  if (!response.ok) {
    throw new Error(`Ошибка удаления изображения: ${response.status}`);
  }
}