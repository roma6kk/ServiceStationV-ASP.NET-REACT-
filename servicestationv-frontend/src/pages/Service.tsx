import React, { useEffect, useState } from 'react';
import '../styles/Service.css';
import Header from '../components/Header';
import Footer from '../components/Footer';
import apiClient from '../apiClient';
import { useParams } from 'react-router-dom';

interface Service {
  id: string;
  name: string;
  description: string;
  price: number;
  imagePath: string;
}

const Service: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [service, setService] = useState<Service | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [isInCart, setIsInCart] = useState<boolean>(false);
  const [isInFavorites, setIsInFavorites] = useState<boolean>(false);

  useEffect(() => {
    const fetchServiceData = async () => {
      try {

        const token = localStorage.getItem('token');
        if (!token) {
          alert('Вы не авторизованы');
          return;
        }


        const serviceResponse = await apiClient.get<Service>(`/services/${id}`);
        setService(serviceResponse.data);


        const cartResponse = await apiClient.get('/cart');
        const cartItems = cartResponse.data;
        setIsInCart(cartItems.some((item: { id: string }) => item.id === serviceResponse.data.id));


        const favoritesResponse = await apiClient.get('/favourites');
        const favoriteIds = favoritesResponse.data.map((s: Service) => s.id);
        setIsInFavorites(favoriteIds.includes(serviceResponse.data.id));

        setLoading(false);
      } catch (err) {
        console.error('Ошибка загрузки данных:', err);
        setError('Не удалось загрузить данные об услуге');
        setLoading(false);
      }
    };

    if (id) {
      fetchServiceData();
    }
  }, [id]);

  const handleCartAction = async () => {
    if (!service) return;

    try {
      if (isInCart) {
        await apiClient.delete(`/cart/${service.id}`);
        setIsInCart(false);
      } else {
        await apiClient.post(`/cart/${service.id}`);
        setIsInCart(true);
      }
    } catch (error) {
      console.error('Ошибка при работе с корзиной:', error);
      alert('Не удалось изменить корзину');
    }
  };

  const handleFavoriteAction = async () => {
    if (!service) return;

    try {
      if (isInFavorites) {
        await apiClient.delete(`/favourites/${service.id}`);
      } else {
        await apiClient.post(`/favourites/${service.id}`, {});
      }
      setIsInFavorites(!isInFavorites);
    } catch (error) {
      console.error('Ошибка при работе с избранным:', error);
      alert('Не удалось изменить избранное');
    }
  };

  if (loading) return <div className="loading-spinner"></div>;
  if (error) return <div className="error-message">{error}</div>;
  if (!service) return <div className="not-found-message">Услуга не найдена</div>;

  return (
    <div className="App">
      <Header />
      <main className="service-container">
        <div className="service-page">
          <div className="service-header">
            <h1 className="service-title">{service.name}</h1>
            <div className="price-badge">{service.price} ₽</div>
          </div>

          <div className="service-content">
            <div className="service-image-container">
              <img 
                src={service.imagePath} 
                alt={service.name} 
                className="service-image"
                loading="lazy"
              />
            </div>

            <div className="service-info">
              <section className="service-description">
                <h2 className="section-title">Описание услуги</h2>
                <p className="description-text">{service.description}</p>
              </section>
              
              <div className="service-actions">
                <button 
                  className={`action-button ${isInCart ? 'danger' : 'primary'}`}
                  onClick={handleCartAction}
                >
                  {isInCart ? 'Удалить из корзины' : 'Добавить в корзину'}
                </button>
                
                <button 
                  className={`action-button ${isInFavorites ? 'favorite' : 'secondary'}`}
                  onClick={handleFavoriteAction}
                >
                  {isInFavorites ? 'В избранном' : 'Добавить в избранное'}
                </button>
              </div>
            </div>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default Service;