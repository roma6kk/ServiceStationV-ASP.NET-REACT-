import React, { useEffect, useState } from 'react';
import '../styles/Cart.css';
import Header from '../components/Header';
import Footer from '../components/Footer';
import apiClient from '../apiClient';
import { motion, AnimatePresence } from 'framer-motion';

interface Service {
  id: string;
  name: string;
  description: string;
  price: number;
  imagePath: string;
}

const Cart = () => {
  const [cartItems, setCartItems] = useState<Service[]>([]);
  const [vehicleInfo, setVehicleInfo] = useState<string>('');
  const [comment, setComment] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [isCheckingOut, setIsCheckingOut] = useState<boolean>(false);
  const [checkoutSuccess, setCheckoutSuccess] = useState<boolean>(false);

  useEffect(() => {
    const fetchCart = async () => {
      try {
        const response = await apiClient.get('/cart');
        setCartItems(response.data || []);
      } catch (err) {
        console.error('Ошибка загрузки корзины:', err);
        setError('Не удалось загрузить данные корзины');
      } finally {
        setLoading(false);
      }
    };

    fetchCart();
  }, []);

  const handleRemoveFromCart = async (serviceId: string) => {
    try {
      await apiClient.delete(`/cart/${serviceId}`);
      setCartItems(prev => prev.filter(item => item.id !== serviceId));
    } catch (error) {
      console.error('Ошибка удаления из корзины:', error);
      alert('Не удалось удалить услугу из корзины');
    }
  };

  const handleCheckout = async () => {
    if (!vehicleInfo.trim()) {
      alert('Введите информацию о транспортном средстве');
      return;
    }
  const serviceIds = cartItems.map(item => item.id);

    try {
      setIsCheckingOut(true);

      const newOrder = {
        vehicleInfo,
        serviceIds,
        totalPrice: total,
        status: null,
        plannedDate: null,
        completedAt: null,
        comment
      };

      await apiClient.post('/orders', newOrder);

      setCheckoutSuccess(true);
      setCartItems([]);
      setVehicleInfo('');
      setComment('');
    } catch (error) {
      console.error('Ошибка оформления заказа:', error);
      alert('Не удалось оформить заказ');
    } finally {
      setIsCheckingOut(false);
    }
  };

  const total = cartItems.reduce((sum, item) => sum + item.price, 0);

  if (loading) {
    return (
      <div className="App">
        <Header />
        <div className="loading-container">
          <div className="loading-spinner"></div>
          <p>Загрузка корзины...</p>
        </div>
        <Footer />
      </div>
    );
  }

  if (error) {
    return (
      <div className="App">
        <Header />
        <div className="error-container">
          <div className="error-icon">⚠️</div>
          <h2>{error}</h2>
          <button 
            className="retry-button"
            onClick={() => window.location.reload()}
          >
            Попробовать снова
          </button>
        </div>
        <Footer />
      </div>
    );
  }

  return (
    <div className="App">
      <Header />
      <main className="cart-container">
        <motion.div 
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
          className="cart-header"
        >
          <h1 className="page-title">Ваша корзина</h1>
          <div className="cart-stats">
            <span>{cartItems.length} {cartItems.length === 1 ? 'товар' : cartItems.length < 5 ? 'товара' : 'товаров'}</span>
            <span className="total-badge">Итого: {total} ₽</span>
          </div>
        </motion.div>

        {checkoutSuccess ? (
          <motion.div
            initial={{ opacity: 0, scale: 0.9 }}
            animate={{ opacity: 1, scale: 1 }}
            className="checkout-success"
          >
            <div className="success-icon">✓</div>
            <h2>Заказ успешно оформлен!</h2>
            <p>Спасибо за ваш заказ. Мы свяжемся с вами в ближайшее время.</p>
            <button 
              className="continue-shopping"
              onClick={() => setCheckoutSuccess(false)}
            >
              Продолжить покупки
            </button>
          </motion.div>
        ) : cartItems.length === 0 ? (
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            className="empty-cart"
          >
            <div className="empty-cart-icon">🛒</div>
            <h2>Ваша корзина пуста</h2>
            <p>Добавьте услуги, чтобы продолжить</p>
            <button 
              className="browse-button"
              onClick={() => window.location.href = '/services'}
            >
              Перейти к услугам
            </button>
          </motion.div>
        ) : (
          <div className="cart-content">
            <div className="cart-items-list">
              <AnimatePresence>
                {cartItems.map((item, index) => (
                  <motion.div
                    key={item.id}
                    initial={{ opacity: 0, x: -20 }}
                    animate={{ opacity: 1, x: 0 }}
                    exit={{ opacity: 0, x: 20 }}
                    transition={{ duration: 0.3, delay: index * 0.05 }}
                    className="cart-item"
                  >
                    <div className="item-image-container">
                      <img 
                        src={item.imagePath} 
                        alt={item.name} 
                        className="item-image"
                        onError={(e) => {
                          e.currentTarget.src = '/images/default.jpg';
                        }}
                      />
                    </div>
                    <div className="item-details">
                      <h3 className="item-name">{item.name}</h3>
                      <p className="item-description">{item.description}</p>
                      <div className="item-footer">
                        <span className="item-price">{item.price} ₽</span>
                        <button 
                          className="remove-item"
                          onClick={() => handleRemoveFromCart(item.id)}
                        >
                          <svg viewBox="0 0 24 24" width="20" height="20">
                            <path d="M19 13H5v-2h14v2z"/>
                          </svg>
                          Удалить
                        </button>
                      </div>
                    </div>
                  </motion.div>
                ))}
              </AnimatePresence>
            </div>

            <motion.div 
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: 0.2 }}
              className="order-summary"
            >
              <h3>Сводка заказа</h3>
              <div className="summary-row">
                <span>Товары ({cartItems.length})</span>
                <span>{total} ₽</span>
              </div>
              <div className="summary-row">
                <span>Доставка</span>
                <span className="free-delivery">Бесплатно</span>
              </div>
              <div className="summary-divider"></div>
              <div className="summary-row total-row">
                <span>Итого</span>
                <span className="total-amount">{total} ₽</span>
              </div>

              <div className="checkout-form">
                <label>
                  Информация о ТС:
                  <input 
                    type="text" 
                    value={vehicleInfo} 
                    onChange={(e) => setVehicleInfo(e.target.value)} 
                    placeholder="Марка, модель, номер..." 
                  />
                </label>

                <label>
                  Комментарий к заказу:
                  <textarea 
                    value={comment} 
                    onChange={(e) => setComment(e.target.value)} 
                    placeholder="Комментарий (необязательно)" 
                  />
                </label>
              </div>

              <button 
                className={`checkout-button ${isCheckingOut ? 'processing' : ''}`}
                onClick={handleCheckout}
                disabled={isCheckingOut}
              >
                {isCheckingOut ? (
                  <>
                    <span className="spinner"></span>
                    Обработка...
                  </>
                ) : (
                  'Оформить заказ'
                )}
              </button>

              <div className="payment-methods">
                <div className="payment-icon visa"></div>
                <div className="payment-icon mastercard"></div>
                <div className="payment-icon mir"></div>
              </div>
            </motion.div>
          </div>
        )}
      </main>
      <Footer />
    </div>
  );
};

export default Cart;
