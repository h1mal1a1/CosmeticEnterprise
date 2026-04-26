import { useEffect, useMemo, useState } from 'react';
import { Link, useSearchParams } from 'react-router-dom';
import { getFinishedProducts } from '../../api/finishedProductsApi';
import { getCategories, type Category } from '../../api/categoriesApi';
import {
  addCartItem,
  getCart,
  removeCartItem,
  updateCartItemQuantity,
} from '../../api/cartApi';
import { ApiError } from '../../types/api';
import type { ShoppingCart } from '../../types/cart';
import type { FinishedProduct } from '../../types/finishedProduct';
import { useAuth } from '../../components/auth/AuthProvider';
import './ProductsPage.css';

function getMainImage(product: FinishedProduct) {
  return product.images.find((x) => x.isMain) ?? product.images[0] ?? null;
}

function formatPrice(value: number): string {
  return new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB',
    minimumFractionDigits: 2,
  }).format(value);
}

export default function ProductsPage() {
  const { isAuthenticated } = useAuth();

  const [searchParams] = useSearchParams();
  const [products, setProducts] = useState<FinishedProduct[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [cart, setCart] = useState<ShoppingCart | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const [cartMessage, setCartMessage] = useState('');
  const [changingProductId, setChangingProductId] = useState<number | null>(null);

  const categoryIdParam = searchParams.get('categoryId');
  const selectedCategoryId = categoryIdParam ? Number(categoryIdParam) : null;

  useEffect(() => {
    async function loadData() {
      try {
        setError('');
        setIsLoading(true);

        const requests: Promise<unknown>[] = [
          getFinishedProducts(),
          getCategories(),
        ];

        if (isAuthenticated) {
          requests.push(getCart());
        }

        const [productsData, categoriesData, cartData] = await Promise.all(requests);

        setProducts(Array.isArray(productsData) ? (productsData as FinishedProduct[]) : []);
        setCategories(Array.isArray(categoriesData) ? (categoriesData as Category[]) : []);
        setCart((cartData as ShoppingCart | undefined) ?? null);
      } catch (err) {
        console.error(err);
        setError('Не удалось загрузить продукцию');
      } finally {
        setIsLoading(false);
      }
    }

    void loadData();
  }, [isAuthenticated]);

  const selectedCategory = useMemo(() => {
    if (selectedCategoryId === null) {
      return null;
    }

    return categories.find((x) => x.id === selectedCategoryId) ?? null;
  }, [categories, selectedCategoryId]);

  const filteredProducts = useMemo(() => {
    const filtered =
      selectedCategoryId === null
        ? products
        : products.filter((x) => x.idProductCategory === selectedCategoryId);

    return filtered
      .slice()
      .sort((a, b) => {
        const aInStock = a.availableQuantity > 0;
        const bInStock = b.availableQuantity > 0;

        if (aInStock === bInStock) {
          return a.name.localeCompare(b.name);
        }

        return aInStock ? -1 : 1;
      });
  }, [products, selectedCategoryId]);

  const cartItemMap = useMemo(() => {
    const map = new Map<number, { itemId: number; quantity: number }>();

    if (!cart) {
      return map;
    }

    for (const item of cart.items) {
      map.set(item.idFinishedProduct, {
        itemId: item.id,
        quantity: item.quantity,
      });
    }

    return map;
  }, [cart]);

  function getAuthRequiredMessage() {
    return 'Для добавления товара в корзину необходимо авторизоваться';
  }

  async function handleAddToCart(
    event: React.MouseEvent<HTMLButtonElement>,
    productId: number,
  ) {
    event.preventDefault();
    event.stopPropagation();

    const product = products.find((x) => x.id === productId);

    if (!product || product.availableQuantity <= 0) {
      setCartMessage('Товара нет в наличии');
      return;
    }

    if (!isAuthenticated) {
      setCartMessage(getAuthRequiredMessage());
      return;
    }

    try {
      setChangingProductId(productId);
      setCartMessage('');

      const updatedCart = await addCartItem({
        idFinishedProduct: productId,
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
      setChangingProductId(null);
    }
  }

  async function handleIncreaseQuantity(
    event: React.MouseEvent<HTMLButtonElement>,
    productId: number,
  ) {
    event.preventDefault();
    event.stopPropagation();

    const product = products.find((x) => x.id === productId);

    if (!product || product.availableQuantity <= 0) {
      setCartMessage('Товара нет в наличии');
      return;
    }

    if (!isAuthenticated) {
      setCartMessage(getAuthRequiredMessage());
      return;
    }

    const cartItem = cartItemMap.get(productId);

    if (!cartItem) {
      await handleAddToCart(event, productId);
      return;
    }

    if (cartItem.quantity >= product.availableQuantity) {
      setCartMessage('В корзине уже максимальное доступное количество товара');
      return;
    }

    try {
      setChangingProductId(productId);
      setCartMessage('');

      const updatedCart = await updateCartItemQuantity(cartItem.itemId, {
        quantity: cartItem.quantity + 1,
      });

      setCart(updatedCart);
    } catch (err) {
      if (err instanceof ApiError && err.status === 401) {
        setCartMessage(getAuthRequiredMessage());
      } else if (err instanceof ApiError) {
        setCartMessage(err.message);
      } else {
        setCartMessage('Не удалось изменить количество товара');
      }
    } finally {
      setChangingProductId(null);
    }
  }

  async function handleDecreaseQuantity(
    event: React.MouseEvent<HTMLButtonElement>,
    productId: number,
  ) {
    event.preventDefault();
    event.stopPropagation();

    if (!isAuthenticated) {
      setCartMessage(getAuthRequiredMessage());
      return;
    }

    const cartItem = cartItemMap.get(productId);

    if (!cartItem) {
      return;
    }

    try {
      setChangingProductId(productId);
      setCartMessage('');

      const updatedCart =
        cartItem.quantity <= 1
          ? await removeCartItem(cartItem.itemId)
          : await updateCartItemQuantity(cartItem.itemId, {
              quantity: cartItem.quantity - 1,
            });

      setCart(updatedCart);
    } catch (err) {
      if (err instanceof ApiError && err.status === 401) {
        setCartMessage(getAuthRequiredMessage());
      } else if (err instanceof ApiError) {
        setCartMessage(err.message);
      } else {
        setCartMessage('Не удалось изменить количество товара');
      }
    } finally {
      setChangingProductId(null);
    }
  }

  if (isLoading) {
    return <div className="products-state">Загрузка продукции...</div>;
  }

  if (error) {
    return <div className="products-state products-state--error">{error}</div>;
  }

  return (
    <div className="products-page">
      <div className="products-header">
        <h1 className="products-title">
          {selectedCategory ? selectedCategory.name : 'Продукция'}
        </h1>
        <p className="products-subtitle">
          {selectedCategory
            ? `Товары категории "${selectedCategory.name}"`
            : 'Вся продукция бренда'}
        </p>

        {cartMessage && (
          <div className="products-cart-message">
            {cartMessage}
          </div>
        )}
      </div>

      {filteredProducts.length === 0 ? (
        <div className="products-state">Товары не найдены.</div>
      ) : (
        <div className="products-grid">
          {filteredProducts.map((product) => {
            const mainImage = getMainImage(product);
            const isChanging = changingProductId === product.id;
            const cartItem = cartItemMap.get(product.id);
            const quantityInCart = cartItem?.quantity ?? 0;
            const isOutOfStock = product.availableQuantity <= 0;

            return (
              <Link
                key={product.id}
                to={`/products/${product.id}`}
                className={
                  isOutOfStock
                    ? 'product-card product-card--out-of-stock'
                    : 'product-card'
                }
              >
                {mainImage ? (
                  <img
                    src={mainImage.fileUrl}
                    alt={product.name}
                    className="product-card__image"
                  />
                ) : (
                  <div className="product-card__image product-card__image--placeholder">
                    Нет фото
                  </div>
                )}

                <div className="product-card__overlay" />

                <div className="product-card__body">
                  <h2 className="product-card__title">{product.name}</h2>

                  <div className="product-card__price">
                    {formatPrice(product.price)}
                  </div>

                  <div className="product-card__actions">
                    <p className="product-card__text">
                      Открыть страницу товара
                    </p>

                    {quantityInCart > 0 ? (
                      <div
                        className="product-card__quantity-control"
                        onClick={(event) => {
                          event.preventDefault();
                          event.stopPropagation();
                        }}
                      >
                        <button
                          type="button"
                          className="product-card__quantity-button"
                          onClick={(event) => void handleDecreaseQuantity(event, product.id)}
                          disabled={isChanging}
                          aria-label="Уменьшить количество"
                        >
                          −
                        </button>

                        <span className="product-card__quantity-value">
                          {quantityInCart}
                        </span>

                        <button
                          type="button"
                          className="product-card__quantity-button"
                          onClick={(event) => void handleIncreaseQuantity(event, product.id)}
                          disabled={isChanging || quantityInCart >= product.availableQuantity}
                          aria-label="Увеличить количество"
                        >
                          +
                        </button>
                      </div>
                    ) : (
                      <button
                        type="button"
                        className="product-card__button"
                        onClick={(event) => void handleAddToCart(event, product.id)}
                        disabled={isChanging || isOutOfStock}
                      >
                        {isOutOfStock
                          ? 'Нет в наличии'
                          : isChanging
                            ? 'Добавление...'
                            : 'В корзину'}
                      </button>
                    )}
                  </div>
                </div>
              </Link>
            );
          })}
        </div>
      )}
    </div>
  );
}