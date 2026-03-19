import type { fileInfo } from "../file/fileInfo";

const backendUrl = 'http://192.168.1.103'
const port = '8080'

export async function uploadFile(
    id: number,
    fileInf: fileInfo
):Promise<void> {
    const formData = new FormData();
    formData.append('id', id.toString());
    formData.append('name', fileInf.name);
    formData.append('data', fileInf.file);

    const url = `${backendUrl}:${port}/api/upload`;

    const controller = new AbortController();
    const timeoutMs = 1000 * 60 * 60 * 1; // 1 час
    const timeoutId = setTimeout(() => controller.abort(), timeoutMs);

    try {
        const response = await fetch(url, {
            method: 'POST', 
            body: formData, 
            signal: controller.signal
        })
        const text = await response.text().catch(() => '');
        if(!response.ok) throw new Error(`Ошибка ${response.status}: ${text}`);
        console.log(text)
    }
    finally {
        clearTimeout(timeoutId);
    }
}

export function uploadFileWithProgress(
  id: number,
  fileInf: fileInfo,
  onProgress: (pct: number) => void,
  timeoutMs: number
): Promise<string> {
  return new Promise((resolve, reject) => {
    const formData = new FormData();
    formData.append('id', id.toString());
    formData.append('name', fileInf.name);
    formData.append('data', fileInf.file);

    const url = `${backendUrl}:${port}/api/upload`;

    const xhr = new XMLHttpRequest();
    xhr.open('POST', url, true);
    xhr.timeout = timeoutMs;

    xhr.upload.onprogress = (e) => {
      if (e.lengthComputable) {
        const pct = Math.round((e.loaded / e.total) * 100);
        onProgress(pct);
      }
    };

    xhr.onload = () => {
      const text = xhr.responseText ?? '';
      if (xhr.status >= 200 && xhr.status < 300) resolve(text);
      else reject(new Error(`Ошибка ${xhr.status}: ${text}`));
    };

    xhr.onerror = () => reject(new Error('Network error'));
    xhr.ontimeout = () => reject(new Error('Timeout'));
    xhr.onabort = () => reject(new Error('Aborted'));

    xhr.send(formData);
  });
}


export async function uploadFiles(files: fileInfo[]):Promise<fileInfo[]> {
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