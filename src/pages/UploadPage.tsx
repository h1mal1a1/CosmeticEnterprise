import './UploadPage.css'
import { useRef, useState } from 'react'
import { type FileInfo } from '../utils/FileInfo';
import { getStatusClass, uploadFile } from '../utils/api';
import { formatFileSize } from '../utils/formatFileSize';
import { useFreeSpace } from '../hooks/useFreeSpace';

export default function UploadPage() {
  const inputFileRef = useRef<HTMLInputElement>(null);
  const [lstFiles, setLstFiles] = useState<FileInfo[]>([]);
  const totalSize = formatFileSize(lstFiles.reduce((sum, file) => sum + file.size, 0))
  const { data: freeSpace } = useFreeSpace();

  const handleButtonClick = () => inputFileRef.current?.click();

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
  };

  return (
  <div className='upload-page-background'>
      <div className='button-row' >
        <div className="info-button">Свободного места: {formatFileSize(freeSpace??NaN)}</div>
        <button className='action-button' onClick={handleButtonClick}> {lstFiles.length > 0 ? `Выбрано файлов: ${lstFiles.length}` : 'Выбрать файлы'}</button>
        <input  type="file" ref={inputFileRef} onChange={handleGetArrayFiles} multiple style={{ display: 'none' }}/>
        <div className='info-button'>Размер фaйлов: {totalSize}</div>
        <button className='action-button' onClick={handleUpload}>Загрузить файлы</button>
      </div>

      {lstFiles.length > 0 && (
        <div className='table-containter'>
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
      )}
  </div>
  );
}