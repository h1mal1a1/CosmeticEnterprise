import { useState } from 'react';
import './ProductsPage.css'

export default function ProductsPage() {
  const [lstProducts, setLstProducts] = useState([]) // Здесь будет логика получения списка товаров

  return (
    <div className='mainDiv'>
      <table>
        <tr>
          <th>Название</th>
          <th>Описание</th>
          <th>Цена</th>
        </tr>
        <tr>
          <td>Товар 1</td>
          <td>Описание товара 1</td>
          <td>100 руб.</td>
        </tr>
      </table>
      <h1>Наша продукция</h1>
      <p>Здесь будут карточки товаров.</p>
    </div>
  );
}