import React, { useState } from 'react';
import '../styles/RegisterForm.css';
import axios from 'axios';
import { API_BASE_URL } from '../constants';
import { useNavigate } from 'react-router-dom';
import { setToken } from '../authSlice';
import { useDispatch } from 'react-redux';

interface LoginFormData {
  phoneNumber: string;
  password: string;
}

interface ValidationError {
  [key: string]: string[];
}

const LoginForm: React.FC = () => {
  const [formData, setFormData] = useState<LoginFormData>({
    phoneNumber: '',
    password: '',
  });

  const [error, setError] = useState<string | null>(null);
  const [validationErrors, setValidationErrors] = useState<ValidationError>({});
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setValidationErrors({});

    try {

      const response = await axios.post(`${API_BASE_URL}Users/login`, formData);


      let token = '';
      if (typeof response.data === 'string') {
        token = response.data;
      } else if (response.data.token) {
        token = response.data.token; 
      }


      if (token) {
        dispatch(setToken(token));
        navigate('/Catalog');
      }
    } catch (err: any) {
      if (err.response && err.response.status === 400) {

        setValidationErrors(err.response.data.errors || {});
      } else if (err.response) {

        setError(`Ошибка входа: ${err.response.statusText}`);
      } else {

        setError('Ошибка сети или сервера. Попробуйте позже.');
      }

      console.error('Ошибка при входе:', err);
    }
  };

  return (
    <div className="register-container">
      <form className="register-form" onSubmit={handleSubmit}>
        <h2>Вход</h2>

        <label>Номер телефона</label>
        {validationErrors.PhoneNumber && (
          <div className="error-message">{validationErrors.PhoneNumber[0]}</div>
        )}
        <input
          type="text"
          name="phoneNumber"
          value={formData.phoneNumber}
          onChange={handleChange}
        />

        <label>Пароль</label>
        {validationErrors.Password && (
          <div className="error-message">{validationErrors.Password[0]}</div>
        )}
        <input
          type="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
        />

        {error && <div className="error-message">{error}</div>}

        <button type="submit">Войти</button>

        <div className="register-link">
          Нет аккаунта?{' '}
          <span className="link-text" onClick={() => navigate('/registration')}>
            Зарегистрироваться
          </span>
        </div>
      </form>
    </div>
  );
};

export default LoginForm;