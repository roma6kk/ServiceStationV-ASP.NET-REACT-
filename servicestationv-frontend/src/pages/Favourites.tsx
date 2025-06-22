import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { API_BASE_URL } from "../constants";
import apiClient from "../apiClient";
import Header from "../components/Header";
import Footer from "../components/Footer";
import "../styles/Favourites.css";

interface Service {
  id: string;
  name: string;
  description: string;
  price: number;
  imagePath: string;
}

function Favourites() {
  const [services, setServices] = useState<Service[]>([]);
  const [sortOrder, setSortOrder] = useState<"asc" | "desc">("asc");
  const [searchTerm, setSearchTerm] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [removingId, setRemovingId] = useState<string | null>(null);

  useEffect(() => {
    const fetchFavourites = async () => {
      setIsLoading(true);
      try {
        const response = await apiClient.get("/favourites");
        setServices(response.data);
      } catch (error) {
        console.error("Ошибка загрузки избранного:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchFavourites();
  }, []);

  const filteredServices = services
    .filter(service =>
      service.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      service.description.toLowerCase().includes(searchTerm.toLowerCase())
    )
    .sort((a, b) =>
      sortOrder === "asc" ? a.price - b.price : b.price - a.price
    );

  const removeFromFavourites = async (serviceId: string) => {
    setRemovingId(serviceId);
    try {
      await apiClient.delete(`/favourites/${serviceId}`);
      setServices(services.filter(s => s.id !== serviceId));
    } catch (error) {
      console.error("Ошибка удаления из избранного:", error);
    } finally {
      setRemovingId(null);
    }
  };

  const toggleSortOrder = () => {
    setSortOrder(prev => prev === "asc" ? "desc" : "asc");
  };

  return (
    <div className="favourites-page">
      <Header />
      <main className="favourites-content">
        <motion.div 
          className="favourites-header"
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
        >
          <h1 className="favourites-title">Избранное</h1>
          <div className="favourites-controls">
            <div className="search-container">
              <input
                type="text"
                placeholder="Поиск в избранном..."
                className="search-input"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
              <svg className="search-icon" viewBox="0 0 24 24">
                <path d="M15.5 14h-.79l-.28-.27a6.5 6.5 0 0 0 1.48-5.34c-.47-2.78-2.79-5-5.59-5.34a6.505 6.505 0 0 0-7.27 7.27c.34 2.8 2.56 5.12 5.34 5.59a6.5 6.5 0 0 0 5.34-1.48l.27.28v.79l4.25 4.25c.41.41 1.08.41 1.49 0 .41-.41.41-1.08 0-1.49L15.5 14zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/>
              </svg>
            </div>
            <button 
              className="sort-button"
              onClick={toggleSortOrder}
            >
              Цена {sortOrder === "asc" ? '↑' : '↓'}
            </button>
          </div>
        </motion.div>

        {isLoading ? (
          <div className="loading-spinner-container">
            <div className="loading-spinner"></div>
          </div>
        ) : filteredServices.length === 0 ? (
          <motion.div
            className="empty-favourites"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ delay: 0.3 }}
          >
            <div className="empty-icon">❤️</div>
            <h2>Ваше избранное пусто</h2>
            <p>Добавляйте понравившиеся услуги, нажимая на сердечко</p>
            <button 
              className="browse-button"
              onClick={() => window.location.href = '/catalog'}
            >
              Перейти к каталогу
            </button>
          </motion.div>
        ) : (
          <div className="favourites-grid">
            <AnimatePresence>
              {filteredServices.map((service) => (
                <motion.div
                  key={service.id}
                  className="favourite-card"
                  layout
                  initial={{ opacity: 0, scale: 0.9 }}
                  animate={{ opacity: 1, scale: 1 }}
                  exit={{ opacity: 0, scale: 0.9, transition: { duration: 0.2 } }}
                  transition={{ type: "spring", stiffness: 300, damping: 25 }}
                  whileHover={{ y: -5, boxShadow: "0 10px 25px rgba(0,0,0,0.1)" }}
                >
                  <div className="card-image-container">
                    <img 
                      src={service.imagePath} 
                      alt={service.name} 
                      className="card-image"
                      loading="lazy"
                    />
                    <button
                      className={`remove-favourite ${removingId === service.id ? 'removing' : ''}`}
                      onClick={() => removeFromFavourites(service.id)}
                      disabled={removingId === service.id}
                      aria-label="Удалить из избранного"
                    >
                      {removingId === service.id ? (
                        <div className="spinner"></div>
                      ) : (
                        <svg viewBox="0 0 24 24">
                          <path fill="currentColor" d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z"/>
                        </svg>
                      )}
                    </button>
                  </div>
                  <div className="card-content">
                    <h3 className="card-title">{service.name}</h3>
                    <p className="card-description">{service.description}</p>
                    <div className="card-footer">
                      <span className="card-price">{service.price.toLocaleString()} ₽</span>
                      <button 
                        className="add-to-cart"
                        onClick={() => console.log('Add to cart', service.id)}
                      >
                        В корзину
                      </button>
                    </div>
                  </div>
                </motion.div>
              ))}
            </AnimatePresence>
          </div>
        )}
      </main>
      <Footer />
    </div>
  );
}

export default Favourites;