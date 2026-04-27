import { env } from '../config/env';
import type { FinishedProductImage } from '../types/finishedProduct';

export async function uploadFinishedProductImages(
  finishedProductId: number,
  files: File[],
): Promise<FinishedProductImage[]> {
  const formData = new FormData();

  files.forEach((file) => {
    formData.append('files', file);
  });

  const response = await fetch(
    `${env.apiBaseUrl}/api/finished-products/${finishedProductId}/images`,
    {
      method: 'POST',
      body: formData,
      credentials: 'include',
    },
  );

  if (!response.ok) {
    throw new Error(`Ошибка загрузки изображений: ${response.status}`);
  }

  return response.json();
}

export async function setMainFinishedProductImage(
  finishedProductId: number,
  imageId: number,
): Promise<FinishedProductImage[]> {
  const response = await fetch(
    `${env.apiBaseUrl}/api/finished-products/${finishedProductId}/images/${imageId}/main`,
    {
      method: 'PATCH',
      credentials: 'include',
    },
  );

  if (!response.ok) {
    throw new Error(`Ошибка установки главного изображения: ${response.status}`);
  }

  return response.json();
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