import { useEffect, useMemo, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { getFinishedProductById } from '../../api/finishedProductsApi';
import { getCategories, type Category } from '../../api/categoriesApi';
import {
  addCartItem,
  getCart,
  removeCartItem,
  updateCartItemQuantity,
} from '../../api/cartApi';
import { ApiError } from '../../types/api';
import type { ShoppingCart } from '../../types/cart';
import type { FinishedProduct, FinishedProductImage } from '../../types/finishedProduct';
import { useAuth } from '../../components/auth/AuthProvider';
import './ProductDetailsPage.css';

function getInitialImage(product: FinishedProduct): FinishedProductImage | null {
  return product.images.find((x) => x.isMain) ?? product.images[0] ?? null;
}

function formatPrice(value: number): string {
  return new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB',
    minimumFractionDigits: 2,
  }).format(value);
}

// 🔥 ВАЖНО: нормализация ссылки
function normalizeUrl(url?: string | null): string {
  if (!url) return '';
  const trimmed = url.trim();

  if (!trimmed) return '';

  if (trimmed.startsWith('http://') || trimmed.startsWith('https://')) {
    return trimmed;
  }

  return `https://${trimmed}`;
}

export default function ProductDetailsPage() {
  const { isAuthenticated } = useAuth();

  const { id } = useParams();
  const productId = id ? Number(id) : null;

  const [product, setProduct] = useState<FinishedProduct | null>(null);
  const [categories, setCategories] = useState<Category[]>([]);
  const [cart, setCart] = useState<ShoppingCart | null>(null);
  const [selectedImage, setSelectedImage] = useState<FinishedProductImage | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const [isChangingCart, setIsChangingCart] = useState(false);
  const [cartMessage, setCartMessage] = useState('');

  useEffect(() => {
    async function loadData() {
      if (!productId || Number.isNaN(productId)) {
        setError('Некорректный идентификатор товара');
        setIsLoading(false);
        return;
      }

      try {
        setError('');
        setIsLoading(true);

        const requests: Promise<unknown>[] = [
          getFinishedProductById(productId),
          getCategories(),
        ];

        if (isAuthenticated) {
          requests.push(getCart());
        }

        const [productData, categoriesData, cartData] = await Promise.all(requests);

        const typedProduct = productData as FinishedProduct;

        setProduct(typedProduct);
        setCategories(categoriesData as Category[]);
        setCart((cartData as ShoppingCart | undefined) ?? null);
        setSelectedImage(getInitialImage(typedProduct));
      } catch {
        setError('Не удалось загрузить страницу товара');
      } finally {
        setIsLoading(false);
      }
    }

    void loadData();
  }, [productId, isAuthenticated]);

  const categoryName = useMemo(() => {
    if (!product) return '';
    return categories.find((x) => x.id === product.idProductCategory)?.name ?? '';
  }, [categories, product]);

  const currentCartItem = useMemo(() => {
    if (!product || !cart) return null;
    return cart.items.find((x) => x.idFinishedProduct === product.id) ?? null;
  }, [cart, product]);

  const quantityInCart = currentCartItem?.quantity ?? 0;
  const isOutOfStock = product ? product.availableQuantity <= 0 : false;

  // 🔥 исправленная ссылка
  const wbUrl = normalizeUrl(product?.wbUrl);

  function getAuthRequiredMessage() {
    return 'Для добавления товара в корзину необходимо авторизоваться';
  }

  async function handleAddToCart() {
    if (!product) return;

    if (product.availableQuantity <= 0) {
      setCartMessage('Товара нет в наличии');
      return;
    }

    if (!isAuthenticated) {
      setCartMessage(getAuthRequiredMessage());
      return;
    }

    try {
      setIsChangingCart(true);
      setCartMessage('');

      const updatedCart = await addCartItem({
        idFinishedProduct: product.id,
        quantity: 1,
      });

      setCart(updatedCart);
      setCartMessage('Товар добавлен в корзину');
    } catch (err) {
      if (err instanceof ApiError && err.status === 401) {
        setCartMessage(getAuthRequiredMessage());
      } else if (err instanceof ApiError) {
        setCartMessage(err.message);
      } else {
        setCartMessage('Не удалось добавить товар в корзину');
      }
    } finally {
      setIsChangingCart(false);
    }
  }

  if (isLoading) {
    return <div className="product-details-state">Загрузка товара...</div>;
  }

  if (error || !product) {
    return (
      <div className="product-details-state product-details-state--error">
        {error || 'Товар не найден'}
      </div>
    );
  }

  return (
    <div className="product-details-page">
      <div className="product-details__back">
        <Link
          to={
            product.idProductCategory
              ? `/products?categoryId=${product.idProductCategory}`
              : '/products'
          }
          className="product-details__back-link"
        >
          ← Назад к товарам
        </Link>
      </div>

      <div className="product-details-card">
        <div className="product-details-gallery">
          <div className="product-details-gallery__main">
            {selectedImage ? (
              <img
                src={selectedImage.fileUrl}
                alt={product.name}
                className="product-details-gallery__main-image"
              />
            ) : (
              <div className="product-details-gallery__placeholder">
                Нет фото
              </div>
            )}
          </div>
        </div>

        <div className="product-details-info">
          <h1 className="product-details-info__title">{product.name}</h1>

          <div className="product-details-info__price">
            {formatPrice(product.price)}
          </div>

          <div className="product-details-info__actions">
            <button
              type="button"
              className="product-details-info__buy-button"
              onClick={() => void handleAddToCart()}
              disabled={isChangingCart || isOutOfStock}
            >
              {isOutOfStock ? 'Нет в наличии' : 'Добавить в корзину'}
            </button>

            {wbUrl && (
              <a
                href={wbUrl}
                target="_blank"
                rel="noreferrer"
                className="product-details-info__wb-link"
              >
                Смотреть на Wildberries
              </a>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}