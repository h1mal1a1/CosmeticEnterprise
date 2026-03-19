const backendUrl = 'http://192.168.1.103'
const port = '8080'

export async function getFreeSpace(): Promise<number> {
    const url = `${backendUrl}:${port}/api/get/freeSpace`;
    const response = await fetch(url, {method: 'GET'});
    if(!response.ok) {
        const errMsg = await response.text().catch(() => 'Неизвестная ошибка');
        throw new Error(`Ошибка ${response.status}: ${errMsg}`);
    }
    // console.log(`tx: ${response.text()}`)
    return await response.json();
}