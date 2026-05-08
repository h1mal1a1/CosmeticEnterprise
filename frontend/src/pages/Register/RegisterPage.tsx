import { useState, type FormEvent } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { register, login } from '../../api/authApi';
import { useAuth } from '../../components/auth/AuthProvider';
import './RegisterPage.css';

const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const phoneRegex = /^\+[1-9]\d{7,14}$/;

export default function RegisterPage() {
  const navigate = useNavigate();
  const { refreshUser } = useAuth();

  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [phone, setPhone] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  const normalizedUsername = username.trim();
  const normalizedEmail = email.trim();
  const normalizedPhone = phone.trim();

  const isEmailValid = emailRegex.test(normalizedEmail);
  const isPhoneValid = phoneRegex.test(normalizedPhone);

  const isFormValid =
    normalizedUsername !== '' &&
    isEmailValid &&
    isPhoneValid &&
    password.trim() !== '' &&
    password === confirmPassword;

  async function handleSubmit(e: FormEvent<HTMLFormElement>) {
    e.preventDefault();

    if (normalizedUsername === '') {
      setError('Введите логин');
      return;
    }

    if (!isEmailValid) {
      setError('Введите корректный email');
      return;
    }

    if (!isPhoneValid) {
      setError('Введите телефон в международном формате, например +79991234567');
      return;
    }

    if (password.trim() === '') {
      setError('Введите пароль');
      return;
    }

    if (password !== confirmPassword) {
      setError('Пароли не совпадают');
      return;
    }

    try {
      setError('');
      setIsLoading(true);

      await register({
        username: normalizedUsername,
        password,
        email: normalizedEmail,
        phone: normalizedPhone,
      });

      await login({
        username: normalizedUsername,
        password,
      });

      await refreshUser();
      navigate('/profile');
    } catch (err: any) {
      setError(err?.message || 'Ошибка регистрации');
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <div className="register-page">
      <div className="register-card">
        <div className="register-card__brand">
          <span className="brand-logo">VALMÉRIS</span>
          <span className="brand-tagline">Присоединяйтесь к нам</span>
        </div>

        <div className="register-card__content">
          <div className="register-card__header">
            <h1>Создать аккаунт</h1>
            <p>Получите доступ к эксклюзивным предложениям и персональному уходу</p>
          </div>

          <form onSubmit={handleSubmit} className="register-form">
            <div className="register-form__field">
              <label htmlFor="username" className="register-form__label">
                Логин
              </label>
              <input
                id="username"
                type="text"
                className="register-form__input"
                placeholder=" "
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                autoComplete="username"
              />
            </div>

            <div className="register-form__field">
              <label htmlFor="email" className="register-form__label">
                Email
              </label>
              <input
                id="email"
                type="email"
                className="register-form__input"
                placeholder=" "
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                autoComplete="email"
              />
            </div>

            <div className="register-form__field">
              <label htmlFor="phone" className="register-form__label">
                Телефон
              </label>
              <input
                id="phone"
                type="tel"
                className="register-form__input"
                placeholder=" "
                value={phone}
                onChange={(e) => setPhone(e.target.value)}
                autoComplete="tel"
              />
            </div>

            <div className="register-form__field">
              <label htmlFor="password" className="register-form__label">
                Пароль
              </label>
              <input
                id="password"
                type="password"
                className="register-form__input"
                placeholder=" "
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                autoComplete="new-password"
              />
            </div>

            <div className="register-form__field">
              <label htmlFor="confirmPassword" className="register-form__label">
                Подтвердите пароль
              </label>
              <input
                id="confirmPassword"
                type="password"
                className="register-form__input"
                placeholder=" "
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                autoComplete="new-password"
              />
            </div>

            <p className="register-form__hint">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <circle cx="12" cy="12" r="10"></circle>
                <line x1="12" y1="16" x2="12" y2="12"></line>
                <line x1="12" y1="8" x2="12.01" y2="8"></line>
              </svg>
              Телефон указывается в международном формате: +, код страны и номер.
            </p>

            {error && (
              <div className="register-error">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                  <circle cx="12" cy="12" r="10"></circle>
                  <line x1="12" y1="8" x2="12" y2="12"></line>
                  <line x1="12" y1="16" x2="12.01" y2="16"></line>
                </svg>
                {error}
              </div>
            )}

            <button type="submit" disabled={!isFormValid || isLoading}>
              {isLoading ? (
                <>
                  <span className="spinner"></span>
                  Регистрация...
                </>
              ) : (
                'Зарегистрироваться'
              )}
            </button>
          </form>

          <div className="register-footer">
            <p>
              Уже есть аккаунт?{' '}
              <Link to="/login" className="register-footer__link">
                Войти
              </Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}