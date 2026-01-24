import type { FileInfo } from "./FileInfo";

const backendUrl = 'http://192.168.1.103'
const port = '8080'

export async function uploadFile(id: number, fileInf: FileInfo):Promise<void> {
    const formData = new FormData();
    formData.append('id', id.toString());
    formData.append('name', fileInf.name);
    formData.append('data', fileInf.file);

    const url = `${backendUrl}:${port}/api/upload`;
    const response = await fetch(url, {method: 'POST', body: formData});
    if(!response.ok) {
        const errMsg = await response.text().catch(() => 'Неизвестная ошибка');
        fileInf.status = 'ошибка';
        throw new Error(`Ошибка ${response.status}: ${errMsg}`);
    }
    console.log(response.text())
}

export async function uploadFiles(files: FileInfo[]):Promise<FileInfo[]> {
    for(let i=0;i<files.length;i++)
        await uploadFile(i, files[i])
    return files;
}

export const getStatusClass = (status: string): string => {
  if (status === 'загружен') return 'status-uploaded';
  if (status === 'ошибка') return 'status-error';
  if (status === 'ожидает' || status === 'загружается') return 'status-pending';
  if (status === 'дубликат') return 'status-copy';
  return '';
};