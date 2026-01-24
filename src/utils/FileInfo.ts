export type FileInfo = {
  file: File;
  name: string;
  size: number;
  status: 'ожидает' | 'загружается' | 'загружен' | 'ошибка' | 'дубликат';
};