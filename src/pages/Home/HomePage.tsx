// src/pages/HomePage.tsx
import './HomePage.css';

export default function HomePage() {
  return (
    <div className="home-container">
      {/* Часть 1: фон — картинка */}
      <section className="home-hero">
        <div className="hero-content">
          <h1>Добро пожаловать в VALMÉRIS</h1>
          <p>Качество, которому доверяют</p>
        </div>
      </section>

      {/* Часть 2: белый фон */}
      <section className="home-content">
        <div className="content-wrapper">
          <h2>О нас</h2>
          <p>Здесь будет описание компании, преимущества, услуги и т.д.</p>
        </div>
      </section>
    </div>
  );
}