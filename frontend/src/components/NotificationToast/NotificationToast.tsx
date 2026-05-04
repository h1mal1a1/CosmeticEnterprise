import { useNotification } from '../../context/NotificationContext';
import './NotificationToast.css';

export default function NotificationToast() {
  const { currentNotification } = useNotification();

  if (!currentNotification) return null;

  const { message, type } = currentNotification;

  const icons = {
    success: '✓',
    error: '✕',
    info: '',
  };

  return (
    <div className={`notification-toast notification-toast--${type}`}>
      <div className="notification-toast__icon">
        {icons[type]}
      </div>
      <span className="notification-toast__message">{message}</span>
    </div>
  );
}