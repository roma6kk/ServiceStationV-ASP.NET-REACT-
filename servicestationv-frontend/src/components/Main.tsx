import React, { useState, useEffect } from "react";
import "../styles/Main.css";
import { API_BASE_URL } from "../constants";
import SortPanel from "./ServiceSorter";
import apiClient from "../apiClient";

interface Service {
  id: string;
  name: string;
  description: string;
  price: number;
  imagePath: string;
}

function Main() {
  const [services, setServices] = useState<Service[]>([]);
  const [sortOrder, setSortOrder] = useState<"asc" | "desc">("asc");
  const [searchTerm, setSearchTerm] = useState("");
  const [favorites, setFavorites] = useState<Set<string>>(new Set());

   useEffect(() => {
    const fetchData = async () => {
      try {
        const servicesResponse = await apiClient.get("/services");
        setServices(servicesResponse.data);

        const favoritesResponse = await apiClient.get("/favourites");
        const favoriteIds = favoritesResponse.data.map((s: Service) => s.id);
        setFavorites(new Set(favoriteIds));
      } catch (error) {
        console.error("Ошибка загрузки данных:", error);
        alert("Не удалось загрузить услуги или избранное");
      }
    };

    const token = localStorage.getItem("token");
    if (!token) {
      alert("Вы не авторизованы");
      return;
    }

    fetchData();
  }, []);

   const filteredServices = services
    .filter(
      (service) =>
        service.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        service.description
          .toLowerCase()
          .includes(searchTerm.toLowerCase())
    )
    .sort((a, b) =>
      sortOrder === "asc" ? a.price - b.price : b.price - a.price
    );


    const toggleFavorite = async (id: string) => {
    if (!localStorage.getItem("token")) {
      alert("Вы не авторизованы");
      return;
    }

    try {
      if (favorites.has(id)) {

        await apiClient.delete(`/favourites/${id}`);
      } else {

        await apiClient.post(`/favourites/${id}`, {});
      }


      setFavorites((prev) => {
        const updated = new Set(prev);
        updated.has(id) ? updated.delete(id) : updated.add(id);
        return updated;
      });
    } catch (error) {
      console.error("Не удалось обновить статус избранного", error);
      alert("Не удалось изменить статус избранного");
    }
  };

  return (
    <main className="main-content">
      <div className="filter-bar">
        <input
          type="text"
          placeholder="Поиск услуг..."
          className="search-input"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <SortPanel
          sortOrder={sortOrder}
          onSortChange={(newOrder) => setSortOrder(newOrder)}
        />
      </div>

      {filteredServices.length === 0 ? (
        <div className="empty-message">
          <p>Ничего не найдено по вашему запросу.</p>
        </div>
      ) : (
        <div className="services-grid">
          {filteredServices.map((service) => (
            <div className="service-card" key={service.id}>
              <div className="card-header">
                <img src={service.imagePath} alt={service.name} />
                <button
                  className="heart-button"
                  onClick={() => toggleFavorite(service.id)}
                  aria-label="Добавить в избранное"
                >
                  {favorites.has(service.id) ? (
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      fill="#e63946"
                      viewBox="0 0 24 24"
                      width="24"
                      height="24"
                    >
                      <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 
                        5.42 4.42 3 7.5 3c1.74 0 3.41 0.81 4.5 2.09C13.09 3.81 
                        14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 
                        11.54L12 21.35z" />
                    </svg>
                  ) : (
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      stroke="#555"
                      strokeWidth="2"
                      viewBox="0 0 24 24"
                      width="24"
                      height="24"
                    >
                      <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 
                        5.42 4.42 3 7.5 3c1.74 0 3.41 0.81 4.5 2.09C13.09 3.81 
                        14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 
                        11.54L12 21.35z" />
                    </svg>
                  )}
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

export default Main;