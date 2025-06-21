import React from "react";
import "../styles/Header.css";
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux"; 
import { USER_ROLE, ADMIN_ROLE } from "../constants";
import { RootState } from "../store";
import { useDispatch } from "react-redux";
import { removeToken } from "../authSlice";

function Header() {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const isAuthenticated = useSelector((state : RootState) => state.auth.isAuthenticated);

  return (
    <header className="header">
      <div className="logo">Service <span>StationV</span></div>

      {isAuthenticated ? (
        <nav className="nav-links">
          <button onClick={() => navigate("/catalog")}>Каталог</button>
          <button onClick={() => navigate("/favourites")}>Избранное</button>
          <button onClick={() => navigate("/cart")}>Корзина</button>
          <button onClick={() => navigate("/profile")}>Профиль</button>
          <button onClick={() => {
            dispatch(removeToken());
            navigate("/login")}}>Выйти</button>
        </nav>
      ) :
      (
        <nav className="nav-links">
          <button onClick={() => navigate("/login")}>Войти</button>
        </nav>
      )
      }
    </header>
  );
}

export default Header;