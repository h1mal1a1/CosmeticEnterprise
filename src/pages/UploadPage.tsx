import './UploadPage.css'
import { useRef, useState, useEffect } from 'react'
import { type FileInfo } from '../utils/FileInfo';
import { getStatusClass, uploadFile } from '../utils/api';
import { formatFileSize } from '../utils/formatFileSize';
import { getFreeSpace } from '../utils/getFreeSpace';
import { useFreeSpace } from '../hooks/useFreeSpace';

export default function UploadPage() {
  const inputFileRef = useRef<HTMLInputElement>(null);
  const [lstFiles, setLstFiles] = useState<FileInfo[]>([]);
  const totalSize = formatFileSize(lstFiles.reduce((sum, file) => sum + file.size, 0))
  const handleButtonClick = () => inputFileRef.current?.click();
  const { data: freeSpace } = useFreeSpace();

  const handleGetArrayFiles = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if(!files) return;
    const newFiles: FileInfo[] = Array.from(files).map(file=>({
      file,
      name: file.name,
      size: file.size,
      status: 'ожидает',
    }));

    setLstFiles(newFiles)
  }
  
    // useEffect(() => {
    //     const fetchFreeSpace = async () => {
    //         try {
                
    //             const space = await getFreeSpace();
    //             setFreeSpaceOnServer(space);
    //         } catch (err) {
    //             console.log(err instanceof Error ? err.message : 'Неизвестная ошибка');
    //         } finally {
                
    //         }
    //     };

    //     fetchFreeSpace();
    // }, []);

  const handleUpload = async () => {
      for (let i = 0; i < lstFiles.length; i++) {
      
        setLstFiles(prev => prev.map((file, idx) => idx === i ? { ...file, status: 'загружается' } : file ));
        try { 
          await uploadFile(i, lstFiles[i]);
          setLstFiles(prev => prev.map((file, idx) =>  idx === i ? { ...file, status: 'загружен' } : file));
        }
        catch (err) {
          console.error(`Ошибка загрузки файла ${lstFiles[i].name}:`, err);
          setLstFiles(prev => prev.map((file, idx) => idx === i ? { ...file, status: 'ошибка' } : file ));
      } 
    }
    
    // setFreeSpaceOnServer(formatFileSize(Number(await getFreeSpace())))
  };

  return (
  <div className='upload-page-background'>
    <div className='file-controls'>
      <div className='d-flex gap-2' >
        <button className="buttons" onClick={(e) => e.preventDefault()} style={{ pointerEvents: 'none' }}>Свободного места: {formatFileSize(freeSpace==null?NaN:freeSpace)}
          
        </button>
        <button className='buttons' onClick={handleButtonClick}> 
                {lstFiles.length > 0 
                  ? `Выбрано файлов: ${lstFiles.length}` 
                  : 'Выбрать файлы'}
              </button>
        <input  type="file" ref={inputFileRef} 
              onChange={handleGetArrayFiles} multiple style={{ display: 'none' }}/>
        <button className="buttons" onClick={(e) => e.preventDefault()} style={{ pointerEvents: 'none' }}>Размер фaйлов: {totalSize}</button>
        {/* Размер фaйлов: {totalSize} */}
        {/* <input type='text' className='file-size-input' readOnly/>Размер файлов: {totalSize} */}
        <button className='buttons' onClick={handleUpload} >Загрузить файлы</button>
      </div>
      <table>
        <thead>
          <tr>
            <th>Имя файла</th>
            <th>Размер</th>
            <th>Статус</th>
          </tr>
        </thead>
        <tbody>
          {lstFiles.map((item,index) => (
          <tr key={index}>
            <td>{item.name}</td>
            <td>{formatFileSize(item.size)}</td>
            <td className={getStatusClass(item.status)}>{item.status}</td>
          </tr>))}
        </tbody>
      </table>
    </div>
  </div>
  );
}