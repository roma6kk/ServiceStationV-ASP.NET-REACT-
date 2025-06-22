import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import { USER_ROLE, ADMIN_ROLE } from "../constants";
import { RootState } from "../store";
import { useDispatch } from "react-redux";
import { removeToken } from "../authSlice";
import "../styles/Header.css";

function Header() {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const isAuthenticated = useSelector((state: RootState) => state.auth.isAuthenticated);
  const [scrolled, setScrolled] = useState(false);
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const userRole = useSelector((state: RootState) => USER_ROLE);

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 10);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  const handleLogout = () => {
    dispatch(removeToken());
    navigate("/login");
  };

  const navItems = [
    { path: "/catalog", label: "Каталог", show: true },
    { path: "/favourites", label: "Избранное", show: isAuthenticated },
    { path: "/cart", label: "Корзина", show: isAuthenticated },
    { path: "/profile", label: "Профиль", show: isAuthenticated },
    { path: "/admin", label: "Админ-панель", show: isAuthenticated && userRole === ADMIN_ROLE },
  ];

  return (
    <motion.header
      className={`header ${scrolled ? "scrolled" : ""}`}
      initial={{ y: -100 }}
      animate={{ y: 0 }}
      transition={{ type: "spring", stiffness: 300, damping: 20 }}
    >
      <div className="header-container">
        <motion.div 
          className="logo"
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          onClick={() => navigate("/")}
        >
          Service <span>Station</span>
          <motion.span 
            className="logo-v"
            initial={{ opacity: 0, x: -10 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ delay: 0.3 }}
          >
            V
          </motion.span>
        </motion.div>

        {/* Desktop Navigation */}
        <nav className="nav-links">
          {navItems.map((item) => (
            item.show && (
              <motion.button
                key={item.path}
                className="nav-button"
                whileHover={{ 
                  y: -2,
                  backgroundColor: "rgba(255, 255, 255, 0.15)"
                }}
                whileTap={{ scale: 0.95 }}
                onClick={() => navigate(item.path)}
              >
                {item.label}
              </motion.button>
            )
          ))}

          {isAuthenticated ? (
            <motion.button
              className="logout-button"
              whileHover={{ 
                y: -2,
                backgroundColor: "rgba(255, 0, 0, 0.2)"
              }}
              whileTap={{ scale: 0.95 }}
              onClick={handleLogout}
            >
              Выйти
            </motion.button>
          ) : (
            <motion.button
              className="login-button"
              whileHover={{ 
                y: -2,
                backgroundColor: "rgba(0, 200, 0, 0.2)"
              }}
              whileTap={{ scale: 0.95 }}
              onClick={() => navigate("/login")}
            >
              Войти
            </motion.button>
          )}
        </nav>

        {/* Mobile Menu Button */}
        <motion.button 
          className="mobile-menu-button"
          onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
          whileTap={{ scale: 0.9 }}
        >
          <motion.div
            animate={mobileMenuOpen ? "open" : "closed"}
            variants={{
              closed: { rotate: 0 },
              open: { rotate: 180 }
            }}
          >
            <svg width="24" height="24" viewBox="0 0 24 24">
              <path 
                d="M3 18H21V16H3V18ZM3 13H21V11H3V13ZM3 6V8H21V6H3Z" 
                fill="white"
              />
            </svg>
          </motion.div>
        </motion.button>
      </div>

      {/* Mobile Menu */}
      <AnimatePresence>
        {mobileMenuOpen && (
          <motion.div 
            className="mobile-menu"
            initial={{ opacity: 0, height: 0 }}
            animate={{ opacity: 1, height: "auto" }}
            exit={{ opacity: 0, height: 0 }}
            transition={{ type: "spring", damping: 25 }}
          >
            {navItems.map((item) => (
              item.show && (
                <motion.button
                  key={`mobile-${item.path}`}
                  className="mobile-nav-button"
                  onClick={() => {
                    navigate(item.path);
                    setMobileMenuOpen(false);
                  }}
                  initial={{ x: -20, opacity: 0 }}
                  animate={{ x: 0, opacity: 1 }}
                  transition={{ delay: 0.1 }}
                >
                  {item.label}
                </motion.button>
              )
            ))}
            {isAuthenticated ? (
              <motion.button
                className="mobile-logout-button"
                onClick={() => {
                  handleLogout();
                  setMobileMenuOpen(false);
                }}
                initial={{ x: -20, opacity: 0 }}
                animate={{ x: 0, opacity: 1 }}
                transition={{ delay: 0.15 }}
              >
                Выйти
              </motion.button>
            ) : (
              <motion.button
                className="mobile-login-button"
                onClick={() => {
                  navigate("/login");
                  setMobileMenuOpen(false);
                }}
                initial={{ x: -20, opacity: 0 }}
                animate={{ x: 0, opacity: 1 }}
                transition={{ delay: 0.15 }}
              >
                Войти
              </motion.button>
            )}
          </motion.div>
        )}
      </AnimatePresence>
    </motion.header>
  );
}

export default Header;