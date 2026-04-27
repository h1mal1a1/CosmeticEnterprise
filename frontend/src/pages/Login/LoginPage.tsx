import { useState, type FormEvent } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { login } from '../../api/authApi';
import { useAuth } from '../../components/auth/AuthProvider';
import './LoginPage.css';

export default function LoginPage() {
  const navigate = useNavigate();
  const { refreshUser } = useAuth();

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  const isFormValid = username.trim() !== '' && password.trim() !== '';

  async function handleSubmit(e: FormEvent<HTMLFormElement>) {
    e.preventDefault();

    if (!isFormValid) {
      setError('Введите логин и пароль');
      return;
    }

    try {
      setError('');
      setIsLoading(true);

      await login({
        username: username.trim(),
        password,
      });

      await refreshUser();
      navigate('/profile');
    } catch {
      setError('Не удалось выполнить вход. Проверьте логин и пароль.');
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <div className="login-page">
      <div className="login-page__background" />

      <div className="login-card">
        <div className="login-card__header">
          <h1>Добро пожаловать</h1>
          <p>Войдите в систему, чтобы продолжить работу</p>
        </div>

        <form className="login-form" onSubmit={handleSubmit}>
          <div className="login-form__group">
            <label htmlFor="username">Логин</label>
            <input
              id="username"
              type="text"
              placeholder="Введите логин"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              autoComplete="username"
            />
          </div>

          <div className="login-form__group">
            <label htmlFor="password">Пароль</label>

            <div className="login-form__password-wrapper">
              <input
                id="password"
                type={showPassword ? 'text' : 'password'}
                placeholder="Введите пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                autoComplete="current-password"
              />

              <button
                type="button"
                className="login-form__password-toggle"
                onClick={() => setShowPassword((prev) => !prev)}
              >
                {showPassword ? 'Скрыть' : 'Показать'}
              </button>
            </div>
          </div>

          {error && <div className="login-form__error">{error}</div>}

          <button
            type="submit"
            className="login-form__submit"
            disabled={!isFormValid || isLoading}
          >
            {isLoading ? 'Вход...' : 'Войти'}
          </button>
        </form>

        <p style={{ marginTop: '16px', textAlign: 'center' }}>
          Нет аккаунта? <Link to="/register">Зарегистрироваться</Link>
        </p>
      </div>
    </div>
  );
}