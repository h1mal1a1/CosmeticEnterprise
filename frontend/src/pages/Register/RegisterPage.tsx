import { useState, type FormEvent } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { register, login } from '../../api/authApi';
import { useAuth } from '../../components/auth/AuthProvider';
import './RegisterPage.css';

export default function RegisterPage() {
  const navigate = useNavigate();
  const { refreshUser } = useAuth();

  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  const isFormValid =
    username.trim() !== '' &&
    password.trim() !== '' &&
    password === confirmPassword;

  async function handleSubmit(e: FormEvent<HTMLFormElement>) {
    e.preventDefault();

    if (!isFormValid) {
      setError('Проверьте корректность введённых данных');
      return;
    }

    try {
      setError('');
      setIsLoading(true);

      await register({
        username: username.trim(),
        password,
        email: email.trim() || undefined,
      });

      // авто-логин после регистрации
      await login({
        username: username.trim(),
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
        <h1>Регистрация</h1>

        <form onSubmit={handleSubmit} className="register-form">
          <input
            placeholder="Логин"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />

          <input
            placeholder="Email (необязательно)"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <input
            type="password"
            placeholder="Пароль"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <input
            type="password"
            placeholder="Повторите пароль"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
          />

          {error && <div className="register-error">{error}</div>}

          <button disabled={!isFormValid || isLoading}>
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