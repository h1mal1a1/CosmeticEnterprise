import type { fileInfo } from "../file/fileInfo";
const backendUrl = 'http://192.168.1.103'
const port = '8080'

export const DEFAULT_CHUNK_SIZE = 8 * 1024 * 1024;

type InitResponse = { uploadId: string }

async function initUpload(
    file: File, 
    chunkSize: number
): Promise<string> {
    const urlBase = `${backendUrl}:${port}`
    const res = await fetch(`${urlBase}/api/upload/init`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            fileName: file.name,
            totalSize: file.size,
            chunkSize, 
        }),
    })

    const text = await res.text().catch(() => "")
    if(!res.ok) throw new Error(`init failed ${res.status}: ${text}`);
    const data = JSON.parse(text) as InitResponse;
    if(!data.uploadId) throw new Error('init response missing uploadId')
    return data.uploadId;    
}

async function uploadChunk(
    uploadId: string, 
    chunkIndex: number,
    chunk: Blob
): Promise<void> {
    const urlBase = `${backendUrl}:${port}`
    const formData = new FormData();
    formData.append('uploadId', uploadId);
    formData.append('chunkIndex', chunkIndex.toString());
    formData.append('data', chunk);

    const res = await fetch(`${urlBase}/api/upload/chunk`, {
        method: 'POST',
        body: formData,
    });

    if(!res.ok) {
        const text = await res.text().catch(() => "")
        throw new Error(`chunk ${chunkIndex} failed ${res.status}: ${text}`);
    }
}

async function completeUpload(
    uploadId: string
): Promise<void> {
    const urlBase = `${backendUrl}:${port}`
    const res = await fetch(`${urlBase}/api/upload/complete`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ uploadId }),
    });

    if(!res.ok) {
        const text = await res.text().catch(() => "")
        throw new Error(`complete failed ${res.status}: ${text}`);
    }
}

export async function uploadFileChunked(
    fileInf: fileInfo,
    onProgress: (pct: number) => void,
    chunkSize: number = DEFAULT_CHUNK_SIZE
): Promise<void> {
    const file = fileInf.file;
    const uploadId = await initUpload(file, chunkSize);
    const totalChunks = Math.ceil(file.size / chunkSize);
    onProgress(0);
    for(let chunkIndex = 0; chunkIndex < totalChunks; chunkIndex++) {
        const start = chunkIndex * chunkSize;
        const end = Math.min(start + chunkSize, file.size);
        const chunk = file.slice(start, end);
        await uploadChunk(uploadId, chunkIndex, chunk);
        const pct = Math.round(((chunkIndex + 1) / totalChunks) * 100);
        onProgress(pct);
    }

    await completeUpload(uploadId);
}