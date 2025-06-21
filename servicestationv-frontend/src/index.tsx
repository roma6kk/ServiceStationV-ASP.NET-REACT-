import React from 'react';
import ReactDOM from 'react-dom/client';
import './styles/index.css';
import Catalog from './pages/Catalog';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import RegisterForm from './pages/RegisterForm';
import LoginForm from './pages/LoginForm';
import HomePage from './pages/Home';
import { Provider } from 'react-redux';
import { store } from './store';
import Favourites from './pages/Favourites';


const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
    <Provider store={store}>
      <Router>
        <Routes>
          <Route path="/login" element={<LoginForm />} />
          <Route path="/registration" element={<RegisterForm />} />
          <Route path="/catalog" element={<Catalog />} />
          <Route path="/about" element={<HomePage />} />
          <Route path="/favourites" element={<Favourites />} />
          <Route path="*" element={<Catalog />} />
        </Routes>
      </Router>
    </Provider>
  </React.StrictMode>
);

