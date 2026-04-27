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
        <div className="register-card__header">
          <h1>Регистрация</h1>
          <p>Создайте аккаунт для оформления заказов</p>
        </div>

        <form onSubmit={handleSubmit} className="register-form">
          <label className="register-form__field">
            <span>Логин</span>
            <input
              type="text"
              placeholder="Введите логин"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              autoComplete="username"
            />
          </label>

          <label className="register-form__field">
            <span>Email</span>
            <input
              type="email"
              placeholder="example@mail.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              autoComplete="email"
            />
          </label>

          <label className="register-form__field">
            <span>Телефон</span>
            <input
              type="tel"
              placeholder="+79991234567"
              value={phone}
              onChange={(e) => setPhone(e.target.value)}
              autoComplete="tel"
            />
          </label>

          <label className="register-form__field">
            <span>Пароль</span>
            <input
              type="password"
              placeholder="Введите пароль"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              autoComplete="new-password"
            />
          </label>

          <label className="register-form__field">
            <span>Повторите пароль</span>
            <input
              type="password"
              placeholder="Повторите пароль"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              autoComplete="new-password"
            />
          </label>

          <p className="register-form__hint">
            Телефон указывается в международном формате: +, код страны и номер.
          </p>

          {error && <div className="register-error">{error}</div>}

          <button type="submit" disabled={!isFormValid || isLoading}>
            {isLoading ? 'Регистрация...' : 'Зарегистрироваться'}
          </button>
        </form>

        <p className="register-footer">
          Уже есть аккаунт? <Link to="/login">Войти</Link>
        </p>
      </div>
    </div>
  );
}