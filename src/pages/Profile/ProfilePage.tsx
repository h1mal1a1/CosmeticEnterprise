import './ProfilePage.css'
import { useRef, useState } from 'react'
import { formatFileSize } from '../../utils/file/formatFileSize'
import { uploadFile } from '../../utils/api/api';

export default function ProfilePage() {
    const inputFileRef = useRef<HTMLInputElement>(null);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [isUploading, setIsUploading] = useState(false);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const [successMessage, setSuccessMessage] = useState(false);

    

    const handleButtonClick = () => {
        if(inputFileRef.current) {
            inputFileRef.current.click()
        }
    };

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0] || null;
        setSelectedFile(file);
        setErrorMessage(null);
        setSuccessMessage(false);
    };

    const handleUpload = async () => {
        if (!selectedFile) return;
        setIsUploading(true);
        setSuccessMessage(false);
        setErrorMessage(null);
        try {
            await uploadFile(Date.now(), { 
              name: selectedFile.name, 
              file: selectedFile,
              size: selectedFile.size,
              status: 'ожидает'
            });
            setSuccessMessage(true);
        } catch (error) {
          if (error instanceof Error) {
            setErrorMessage(error.message);
          } else {
            setErrorMessage('Неизвестная ошибка');
          }
        } finally {
            setIsUploading(false);
        }
    }
  return (
  <div>
    <h1>Страница личного кабинета</h1>

    <div className='file-controls'>
      <button onClick={handleButtonClick}>
        {selectedFile? selectedFile.name : 'Выбрать файл'}
      </button>

      {selectedFile && (
        <>
          <input 
            type='text'
            value={formatFileSize(selectedFile.size)}
            readOnly
            className='file-size-input'/>
          <button onClick={handleUpload} disabled={isUploading}>
            {isUploading ? 'Загрузка...' : 'Отправить на сервер'}
          </button>
        </>
      )}
    </div>
    
      {successMessage && (
        <p style={{ color: 'green', marginTop: '10px' }}>
          ✅ Файл успешно загружен!
        </p>
      )}

      {/* Показываем ошибку */}
      {errorMessage && (
        <p style={{ color: 'red', marginTop: '10px', whiteSpace: 'pre-wrap' }}>
          ❌ Ошибка: {errorMessage}
        </p>
      )}

    <input 
      type='file'
      ref={inputFileRef}
      onChange={handleFileChange}
      style={{display: 'none'}}
    />
  </div>
  );
}