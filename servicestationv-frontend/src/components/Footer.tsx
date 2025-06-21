import React from "react";
import "../styles/Footer.css";
import { useNavigate } from "react-router-dom";


function Footer() {
    const navigate = useNavigate();
  return (
    <footer>
      <nav className="footer-nav">
        <a href="#" onClick={() => navigate("/catalog")}>Каталог</a>
        <a href="#">Профиль</a>
        <a href="#" className="active">SERVICE</a>
        <a href="#" onClick={() => navigate("/about")}>О нас</a>
        <a href="#">Корзина</a>
      </nav>
      <p>&copy; 2010 – 2025 Service Station V. Все права защищены.</p>
    </footer>
  );
}

export default Footer;
