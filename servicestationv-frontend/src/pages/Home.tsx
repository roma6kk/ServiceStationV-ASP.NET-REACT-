import React from "react";
import "../styles/Home.css";
import Header from "../components/Header";
import Footer from "../components/Footer";

function HomePage() {
  return (
    <div className="home-page">
      <Header />

      <div className="banner">
        <img src="/ServicesImages/RadiatorCheck.jpeg" alt="Car Repair" className="banner-image" />
      </div>

      <section className="about-section">
        <h2>Немного о нас</h2>
        <div className="about-content">
          <div className="about-block">
            <img src="/ServicesImages/EnigneDiagnostic.jpeg" alt="Mechanic" />
            <p>
              Welcome to your trusted partner in vehicle care. With years of
              experience and a team of certified technicians, we provide
              top-quality auto repair and maintenance services for all makes and
              models.
            </p>
          </div>
          <div className="about-block">
            <p>
              Our mission is to deliver reliable, honest, and efficient service
              to keep your vehicle running safely and smoothly. Whether it’s a
              routine oil change, brake repair, engine diagnostics, or
              scheduled maintenance — we’ve got you covered.
            </p>
            <img src="/ServicesImages/MegaphoneInstallation.jpeg" alt="Driving" />
          </div>
        </div>
      </section>

      <section className="services-section">
        <h2>Популярные услуги</h2>
        <div className="service-cards">
<div className="service-card">
  <div className="card-header">
    <img src="/ServicesImages/MegaphoneInstallation.jpeg" alt="Service Image"/>
    <button className="heart-button" aria-label="Добавить в избранное">
      {/* <!-- НЕАКТИВНОЕ сердце --> */}
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" stroke="#555" stroke-width="2" viewBox="0 0 24 24" width="24" height="24">
        <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 
        12.28 2 8.5 2 5.42 4.42 3 
        7.5 3c1.74 0 3.41 0.81 4.5 
        2.09C13.09 3.81 14.76 3 
        16.5 3 19.58 3 22 5.42 22 
        8.5c0 3.78-3.4 6.86-8.55 
        11.54L12 21.35z"/>
      </svg>
    </button>
  </div>
  <h3>Название услуги</h3>
  <p>Краткое описание услуги, которое дает общее представление о её содержании.</p>
  <div className="service-details">
    <span>3000 ₽</span>
  </div>
</div>

         <div className="service-card">
  <div className="card-header">
    <img src="/ServicesImages/MegaphoneInstallation.jpeg" alt="Service Image"/>
    <button className="heart-button" aria-label="Добавить в избранное">
      {/* <!-- НЕАКТИВНОЕ сердце --> */}
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" stroke="#555" stroke-width="2" viewBox="0 0 24 24" width="24" height="24">
        <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 
        12.28 2 8.5 2 5.42 4.42 3 
        7.5 3c1.74 0 3.41 0.81 4.5 
        2.09C13.09 3.81 14.76 3 
        16.5 3 19.58 3 22 5.42 22 
        8.5c0 3.78-3.4 6.86-8.55 
        11.54L12 21.35z"/>
      </svg>
    </button>
  </div>
  <h3>Название услуги</h3>
  <p>Краткое описание услуги, которое дает общее представление о её содержании.</p>
  <div className="service-details">
    <span>3000 ₽</span>
  </div>
</div>
          <div className="service-card">
  <div className="card-header">
    <img src="/ServicesImages/MegaphoneInstallation.jpeg" alt="Service Image"/>
    <button className="heart-button" aria-label="Добавить в избранное">
      {/* <!-- НЕАКТИВНОЕ сердце --> */}
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" stroke="#e63946" stroke-width="2" viewBox="0 0 24 24" width="24" height="24">
        <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 
        12.28 2 8.5 2 5.42 4.42 3 
        7.5 3c1.74 0 3.41 0.81 4.5 
        2.09C13.09 3.81 14.76 3 
        16.5 3 19.58 3 22 5.42 22 
        8.5c0 3.78-3.4 6.86-8.55 
        11.54L12 21.35z"/>
      </svg>
    </button>
  </div>
  <h3>Название услуги</h3>
  <p>Краткое описание услуги, которое дает общее представление о её содержании.</p>
  <div className="service-details">
    <span>3000 ₽</span>
  </div>
</div>
        </div>
      </section>

      <section className="reviews-section">
        <h2>Наши отзывы</h2>
        <div className="review-cards">
          <div className="review-card">
            <h4>🧑‍🔧 Alice</h4>
            <p>This is a fantastic product! Highly recommend it.</p>
          </div>
          <div className="review-card">
            <h4>🧑‍🔧 Alice</h4>
            <p>This is a fantastic product! Highly recommend it.</p>
          </div>
          <div className="review-card">
            <h4>🧑‍🔧 Alice</h4>
            <p>This is a fantastic product! Highly recommend it.</p>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  );
}

export default HomePage;
