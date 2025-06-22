import React from "react";
import { Link } from "react-router-dom";

interface Service {
  id: string;
  name: string;
  description: string;
  price: number;
  imagePath: string;
}

interface ServiceCardProps {
  service: Service;
  isFavorite: boolean;
  onToggleFavorite: () => void;
}

const ServiceCard: React.FC<ServiceCardProps> = ({
  service,
  isFavorite,
  onToggleFavorite,
}) => {
  return (
    <Link
      to={`/services/${service.id}`}
      className="service-card-link"
      onClick={(e) => {
        if ((e.target as HTMLElement).closest(".heart-button")) {
          e.preventDefault();
        }
      }}
    >
      <div className="service-card">
        <div className="card-header">
          <img src={service.imagePath} alt={service.name} />
          <button
            className="heart-button"
            onClick={onToggleFavorite}
            aria-label="Добавить в избранное"
          >
            {isFavorite ? (
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
    </Link>
  );
};

export default ServiceCard;
