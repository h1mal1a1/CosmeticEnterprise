import { useEffect, useState } from 'react';
import './ScrollToTopButton.css';

interface ScrollToTopButtonProps {
  offset?: number;
}

export default function ScrollToTopButton({ offset = 300 }: ScrollToTopButtonProps) {
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    function toggleVisibility() {
      if (window.scrollY > offset) {
        setIsVisible(true);
      } else {
        setIsVisible(false);
      }
    }

    window.addEventListener('scroll', toggleVisibility);
    return () => window.removeEventListener('scroll', toggleVisibility);
  }, [offset]);

  function scrollToTop() {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  }

  if (!isVisible) return null;

  return (
    <button
      type="button"
      className="scroll-to-top-button"
      onClick={scrollToTop}
      aria-label="Вернуться наверх"
      title="Наверх"
    >
      ↑
    </button>
  );
}