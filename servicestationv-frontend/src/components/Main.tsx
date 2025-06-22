import React, { useState, useEffect } from "react";
import "../styles/Main.css";
import { useSelector } from "react-redux";
import { RootState } from "../store";
import apiClient from "../apiClient";
import SearchBar from "./SearchBar";
import ServiceCard from "./ServiceCard";

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
  const [loading, setLoading] = useState<boolean>(false);

  const isAuthenticated = useSelector((state: RootState) => state.auth.isAuthenticated);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true); // старт загрузки
      try {
        const servicesResponse = await apiClient.get("/services");
        setServices(servicesResponse.data);
        if (isAuthenticated) {
          const favoritesResponse = await apiClient.get("/favourites");
          const favoriteIds = favoritesResponse.data.map((s: Service) => s.id);
          setFavorites(new Set(favoriteIds));
        }
      } catch (error: any) {
        if (error.response) {
          const status = error.response.status;
          switch (status) {
            case 400:
              alert("Некорректный запрос.");
              break;
            case 404:
              alert("Ресурс не найден.");
              break;
            case 500:
              alert("Внутренняя ошибка сервера. Попробуйте позже.");
              break;
            default:
              alert(`Ошибка сервера: ${status}. Попробуйте снова.`);
          }
        } else if (error.request) {
          alert("Сервер не отвечает. Проверьте соединение.");
        } else {
          alert("Произошла ошибка: " + error.message);
        }
      } finally {
        setLoading(false); // окончание загрузки
      }
    };

    fetchData();
  }, [isAuthenticated]);

  const filteredServices = services
    .filter(
      (service) =>
        service.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        service.description.toLowerCase().includes(searchTerm.toLowerCase())
    )
    .sort((a, b) =>
      sortOrder === "asc" ? a.price - b.price : b.price - a.price
    );

  const toggleFavorite = async (id: string) => {
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
    } catch (error: any) {
      if (error.response) {
        const status = error.response.status;
        switch (status) {
          case 400:
            alert("Некорректный запрос.");
            break;
          case 401:
            alert("Вы не авторизованы. Пожалуйста, войдите в систему.");
            break;
          case 404:
            alert("Ресурс не найден.");
            break;
          case 500:
            alert("Внутренняя ошибка сервера. Попробуйте позже.");
            break;
          default:
            alert(`Ошибка сервера: ${status}. Попробуйте снова.`);
        }
      } else if (error.request) {
        alert("Сервер не отвечает. Проверьте соединение.");
      } else {
        alert("Произошла ошибка: " + error.message);
      }
    }
  };

  return (
    <main className="main-content">
      <SearchBar
        searchTerm={searchTerm}
        onSearchChange={setSearchTerm}
        sortOrder={sortOrder}
        onSortChange={setSortOrder}
      />

      {loading ? (
        <div className="loading-indicator">
          <p>Загрузка данных...</p>
          {/* Или можно использовать spinner */}
        </div>
      ) : filteredServices.length === 0 ? (
        <div className="empty-message">
          <p>Ничего не найдено по вашему запросу.</p>
        </div>
      ) : (
        <div className="services-grid">
          {filteredServices.map((service) => (
            <ServiceCard
              key={service.id}
              service={service}
              isFavorite={favorites.has(service.id)}
              onToggleFavorite={() => toggleFavorite(service.id)}
            />
          ))}
        </div>
      )}
    </main>
  );
}

export default Main;
