import React, { useState } from 'react';
import '../styles/RegisterForm.css';
import axios from 'axios';
import { API_BASE_URL } from '../constants';
import { useNavigate, Link } from 'react-router-dom';

interface FormData {
  userName: string;
  email: string;
  phoneNumber: string;
  password: string;
}

interface ValidationError {
  [key: string]: string[];
}

const RegisterForm: React.FC = () => {
  const [formData, setFormData] = useState<FormData>({
    userName: '',
    email: '',
    phoneNumber: '',
    password: '',
  });

  const [validationErrors, setValidationErrors] = useState<ValidationError>({});
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setValidationErrors({});
    setError(null);

    try {

      const response = await axios.post(`${API_BASE_URL}Users/register`, formData);


      navigate('/login');
    } catch (err: any) {
      if (err.response && err.response.status === 400) {

        setValidationErrors(err.response.data.errors);
      } else if (err.response) {

        setError(`Ошибка регистрации: ${err.response.statusText}`);
      } else {

        setError('Ошибка сети или сервера. Попробуйте позже.');
      }

      console.error('Ошибка регистрации:', err);
    }
  };

  return (
    <div className="register-container">
      <form className="register-form" onSubmit={handleSubmit}>
        <h2>Регистрация</h2>

        <label>Имя пользователя</label>
        {validationErrors.UserName && (
          <div className="error-message">{validationErrors.UserName[0]}</div>
        )}
        <input
          type="text"
          name="userName"
          value={formData.userName}
          onChange={handleChange}
        />

        <label>Email</label>
        {validationErrors.Email && (
          <div className="error-message">{validationErrors.Email[0]}</div>
        )}
        <input
          type="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
        />

        <label>Телефон</label>
        {validationErrors.PhoneNumber && (
          <div className="error-message">{validationErrors.PhoneNumber[0]}</div>
        )}
        <input
          type="tel"
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

        <button type="submit">Зарегистрироваться</button>

        <div className="form-footer">
          Уже есть аккаунт? <Link to="/login">Войти</Link>
        </div>
      </form>
    </div>
  );
};

export default RegisterForm;