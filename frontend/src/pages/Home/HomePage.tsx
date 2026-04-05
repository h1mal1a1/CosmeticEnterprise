import { useEffect, useState } from 'react';
import './HomePage.css';

const siberiaImages = [
  '/altai/1.jpg',
  '/altai/2.jpg',
  '/altai/3.jpg',
];

const parisImages = [
  '/parij/1.jpg',
  '/parij/2.jpg',
  '/parij/3.jpg',
];

export default function HomePage() {
  const [siberiaIndex, setSiberiaIndex] = useState(0);
  const [parisIndex, setParisIndex] = useState(0);

  useEffect(() => {
    const siberiaTimer = window.setInterval(() => {
      setSiberiaIndex((prev) => (prev + 1) % siberiaImages.length);
    }, 4000);

    const parisTimer = window.setInterval(() => {
      setParisIndex((prev) => (prev + 1) % parisImages.length);
    }, 4500);

    return () => {
      window.clearInterval(siberiaTimer);
      window.clearInterval(parisTimer);
    };
  }, []);

  return (
    <div className="home">
      {/* HERO */}
      <section className="hero">
        <div className="hero-overlay" />

        <div className="hero-content">
          <h1>
            VALMÉRIS: Алхимия Сибири и Парижа для вашего наследия красоты
          </h1>

          <p>
            Где рождается подлинная эффективность? На стыке двух миров. Из
            мощи сибирских кедров и точности парижских формул рождается уход,
            который работает.
          </p>
        </div>
      </section>

      {/* TITLE */}
      <section className="section-title">
        СИЛА, У КОТОРОЙ ЕСТЬ АДРЕС
      </section>

      {/* SIBERIA */}
      <section className="content-block">
        <div className="content-image">
          <img
            src={siberiaImages[siberiaIndex]}
            alt="Сибирь"
            className="rotating-image"
          />
        </div>

        <div className="content-text">
          <h2>Сибирь | Алтай</h2>

          <p>
            Мы не добываем сырье — мы перенимаем дар. Наше производство
            расположено в сердце Алтая, чтобы сохранить жизненную силу
            растений.
          </p>

          <ul>
            <li>Живица кедра — природный антисептик</li>
            <li>Родиола розовая — адаптоген</li>
            <li>Мумие — природное соединение</li>
          </ul>
        </div>
      </section>

      {/* PARIS */}
      <section className="content-block reverse">
        <div className="content-text">
          <h2>Париж | Грас</h2>

          <p>
            Сила природы недостаточна — нужна виртуозность. В лабораториях
            Граса создаются формулы, усиливающие природные компоненты.
          </p>

          <ul>
            <li>Инновационные системы доставки</li>
            <li>Биодоступность активов</li>
            <li>Элегантность текстур</li>
          </ul>
        </div>

        <div className="content-image">
          <img
            src={parisImages[parisIndex]}
            alt="Париж"
            className="rotating-image"
          />
        </div>
      </section>
    </div>
  );
}