import { createContext, useContext, useState, useCallback, useEffect, type ReactNode } from 'react';

type NotificationType = 'success' | 'error' | 'info';

interface NotificationData {
  message: string;
  type: NotificationType;
  id: number;
}

interface NotificationContextType {
  showNotification: (message: string, type?: NotificationType) => void;
  currentNotification: NotificationData | null;
}

const NotificationContext = createContext<NotificationContextType | undefined>(undefined);

export function useNotification() {
  const context = useContext(NotificationContext);
  if (!context) {
    throw new Error('useNotification must be used within a NotificationProvider');
  }
  return context;
}

interface NotificationProviderProps {
  children: ReactNode;
}

export function NotificationProvider({ children }: NotificationProviderProps) {
  const [notification, setNotification] = useState<NotificationData | null>(null);

  const showNotification = useCallback((message: string, type: NotificationType = 'success') => {
    const id = Date.now();
    setNotification({ message, type, id });
  }, []);

  useEffect(() => {
    if (notification) {
      const timer = setTimeout(() => {
        setNotification(null);
      }, 3000);
      return () => clearTimeout(timer);
    }
  }, [notification]);

  return (
    <NotificationContext.Provider value={{ showNotification, currentNotification: notification }}>
      {children}
    </NotificationContext.Provider>
  );
}