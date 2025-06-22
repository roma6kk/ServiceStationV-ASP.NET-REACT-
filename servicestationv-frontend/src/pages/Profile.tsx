import React, { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from '../store';
import apiClient from '../apiClient';
import Header from '../components/Header';
import Footer from '../components/Footer';
import '../styles/Profile.css';

interface UserData {
  id: string;
  userName: string;
  email: string;
  phoneNumber: string;
  role: string;
  avatarUrl?: string;
}

const Profile: React.FC = () => {
  const dispatch = useDispatch();
  const [userData, setUserData] = useState<UserData | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [formData, setFormData] = useState({
    userName: '',
    email: '',
    phoneNumber: ''
  });
  const [avatar, setAvatar] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const { isAuthenticated, userId } = useSelector((state: RootState) => state.auth);

  useEffect(() => {
    const fetchUserData = async () => {
      if (!isAuthenticated || !userId) {
        setError('Пользователь не авторизован');
        setIsLoading(false);
        return;
      }

      try {
        const response = await apiClient.get(`/users`);
        setUserData(response.data);
        setFormData({
          userName: response.data.userName,
          email: response.data.email,
          phoneNumber: response.data.phoneNumber
        });
        if (response.data.avatarUrl) {
          setPreviewUrl(response.data.avatarUrl);
        }
      } catch (err) {
        console.error('Ошибка загрузки данных:', err);
        setError('Не удалось загрузить данные пользователя');
      } finally {
        setIsLoading(false);
      }
    };

    fetchUserData();
  }, [isAuthenticated, userId]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleAvatarChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const file = e.target.files[0];
      setAvatar(file);
      
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreviewUrl(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);
    setSuccessMessage(null);

    try {
      const formDataToSend = new FormData();
      formDataToSend.append('userName', formData.userName);
      formDataToSend.append('email', formData.email);
      formDataToSend.append('phoneNumber', formData.phoneNumber);
      if (avatar) {
        formDataToSend.append('avatar', avatar);
      }

      const response = await apiClient.put(`/users/${userId}`, formDataToSend, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });

      setUserData(response.data);
      if (response.data.avatarUrl) {
        setPreviewUrl(response.data.avatarUrl);
      }
      setSuccessMessage('Данные успешно обновлены');
      setIsEditing(false);
    } catch (err) {
      console.error('Ошибка обновления данных:', err);
      setError('Не удалось обновить данные');
    } finally {
      setIsLoading(false);
    }
  };

  if (!isAuthenticated) {
    return (
      <div className="profile-container">
        <Header />
        <main className="not-authorized">
          <h2>Для просмотра профиля необходимо авторизоваться</h2>
          <button 
            className="auth-button"
            onClick={() => window.location.href = '/login'}
          >
            Войти
          </button>
        </main>
        <Footer />
      </div>
    );
  }

  if (isLoading && !userData) {
    return (
      <div className="profile-container">
        <Header />
        <main className="loading-container">
          <div className="loading-spinner"></div>
        </main>
        <Footer />
      </div>
    );
  }

  if (error) {
    return (
      <div className="profile-container">
        <Header />
        <main className="error-container">
          <div className="error-icon">⚠️</div>
          <h2>{error}</h2>
          <button 
            className="retry-button"
            onClick={() => window.location.reload()}
          >
            Попробовать снова
          </button>
        </main>
        <Footer />
      </div>
    );
  }

  return (
    <div className="profile-container">
      <Header />
      <main className="profile-content">
        <motion.div 
          className="profile-header"
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
        >
          <h1>Мой профиль</h1>
          {userData?.role === 'admin' && (
            <span className="admin-badge">Администратор</span>
          )}
        </motion.div>

        <div className="profile-card">
          <div className="avatar-section">
            <div className="avatar-container">
              {previewUrl ? (
                <img 
                  src={previewUrl} 
                  alt="Аватар пользователя" 
                  className="avatar-image"
                />
              ) : (
                <div className="avatar-placeholder">
                  {userData?.userName.charAt(0).toUpperCase()}
                </div>
              )}
              {isEditing && (
                <div className="avatar-overlay">
                  <label htmlFor="avatar-upload" className="avatar-upload-label">
                    <svg viewBox="0 0 24 24" width="24" height="24">
                      <path fill="white" d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z"/>
                    </svg>
                    <input
                      id="avatar-upload"
                      type="file"
                      accept="image/*"
                      onChange={handleAvatarChange}
                      className="avatar-upload-input"
                    />
                  </label>
                </div>
              )}
            </div>
          </div>

          {isEditing ? (
            <motion.form
              onSubmit={handleSubmit}
              className="profile-form"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ duration: 0.3 }}
            >
              <div className="form-group">
                <label htmlFor="userName">Имя пользователя</label>
                <input
                  type="text"
                  id="userName"
                  name="userName"
                  value={formData.userName}
                  onChange={handleInputChange}
                  maxLength={30}
                  required
                />
              </div>

              <div className="form-group">
                <label htmlFor="email">Email</label>
                <input
                  type="email"
                  id="email"
                  name="email"
                  value={formData.email}
                  onChange={handleInputChange}
                  maxLength={30}
                  required
                />
              </div>

              <div className="form-group">
                <label htmlFor="phoneNumber">Телефон</label>
                <input
                  type="tel"
                  id="phoneNumber"
                  name="phoneNumber"
                  value={formData.phoneNumber}
                  onChange={handleInputChange}
                  maxLength={12}
                />
              </div>

              {error && <div className="error-message">{error}</div>}
              {successMessage && <div className="success-message">{successMessage}</div>}

              <div className="form-actions">
                <button
                  type="button"
                  className="cancel-button"
                  onClick={() => setIsEditing(false)}
                  disabled={isLoading}
                >
                  Отмена
                </button>
                <button
                  type="submit"
                  className="save-button"
                  disabled={isLoading}
                >
                  {isLoading ? 'Сохранение...' : 'Сохранить изменения'}
                </button>
              </div>
            </motion.form>
          ) : (
            <motion.div
              className="profile-info"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ duration: 0.3 }}
            >
              <div className="info-group">
                <span className="info-label">Имя пользователя:</span>
                <span className="info-value">{userData?.userName}</span>
              </div>

              <div className="info-group">
                <span className="info-label">Email:</span>
                <span className="info-value">{userData?.email}</span>
              </div>

              <div className="info-group">
                <span className="info-label">Телефон:</span>
                <span className="info-value">
                  {userData?.phoneNumber || 'Не указан'}
                </span>
              </div>

              <div className="info-group">
                <span className="info-label">Роль:</span>
                <span className="info-value">
                  {userData?.role === 'admin' ? 'Администратор' : 'Пользователь'}
                </span>
              </div>

              {successMessage && <div className="success-message">{successMessage}</div>}

              <button
                className="edit-button"
                onClick={() => setIsEditing(true)}
              >
                Редактировать профиль
              </button>
            </motion.div>
          )}
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default Profile;