import React, { useState, useEffect } from "react";
import "../styles/Main.css";
import axios from "axios";
import { API_BASE_URL } from "../constants";
import apiClient from "../apiClient";

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

  useEffect(() => {
  const fetchFavourites = async () => {
    const token = localStorage.getItem("token");

    if (!token) {
      console.error("Токен отсутствует");
      return;
    }

    try {
      const response = await apiClient.get("/favourites"); // если используешь apiClient
      setServices(response.data);
    } catch (error) {
      console.error("Ошибка загрузки избранного:", error);
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
    try {
      await axios.delete(`${API_BASE_URL}favourites/${serviceId}`);
      setServices(services.filter(s => s.id !== serviceId));
    } catch (error) {
      console.error("Ошибка удаления из избранного:", error);
    }
  };

  return (
    <main className="main-content">
      <h2>Избранное</h2>

      <div className="filter-bar">
        <input
          type="text"
          placeholder="Поиск в избранном..."
          className="search-input"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>

      {filteredServices.length === 0 ? (
        <div className="empty-message">
          <p>В избранном пока ничего нет.</p>
        </div>
      ) : (
        <div className="services-grid">
          {filteredServices.map((service) => (
            <div className="service-card" key={service.id}>
              <div className="card-header">
                <img src={service.imagePath} alt={service.name} />
                <button
                  className="heart-button"
                  onClick={() => removeFromFavourites(service.id)}
                  aria-label="Удалить из избранного"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="#e63946"
                    viewBox="0 0 24 24"
                    width="24"
                    height="24"
                  >
                    <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 
                      5.42 4.42 3 7.5 3c1.74 0 3.41 
                      0.81 4.5 2.09C13.09 3.81 14.76 
                      3 16.5 3 19.58 3 22 5.42 22 
                      8.5c0 3.78-3.4 6.86-8.55 
                      11.54L12 21.35z" />
                  </svg>
                </button>
              </div>
              <h3>{service.name}</h3>
              <p>{service.description}</p>
              <div className="service-details">
                <span>{service.price} ₽</span>
              </div>
            </div>
          ))}
        </div>
      )}
    </main>
  );
}

export default Favourites;