export async function uploadFile(id: number, fileName: string, file: File):Promise<void> {
    const formData = new FormData;
    formData.append('id',id.toString());
    formData.append('name',fileName);
    formData.append('data',file);

    const response = await fetch('http://192.168.1.103:8080/api/upload', {
        method: 'POST', 
        body: formData
    });
    if(!response.ok) {
        const errMsg = await response.text().catch(() => 'Неизвестная ошибка');
        throw new Error(`Ошибка ${response.status}: ${errMsg}`);
    }
}