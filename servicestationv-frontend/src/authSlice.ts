import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import {jwtDecode} from 'jwt-decode';

interface JwtPayload {
  userId: string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
  exp: number;
}

interface AuthState {
  token: string | null;
  isAuthenticated: boolean;
  role: string | null;
  userId: string | null;
}

const initialState: AuthState = {
  token: localStorage.getItem('token') || null,
  isAuthenticated: !!localStorage.getItem('token'),
  role: null,
  userId: null,
};

// Функция для декодирования токена и извлечения данных
const decodeToken = (token: string) => {
  try {
    const decoded = jwtDecode<JwtPayload>(token);
    return {
      role: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
      userId: decoded.userId
    };
  } catch (error) {
    console.error('Ошибка декодирования токена:', error);
    return { role: null, userId: null };
  }
};

// Проверяем и инициализируем данные из токена при загрузке
if (initialState.token) {
  const { role, userId } = decodeToken(initialState.token);
  initialState.role = role;
  initialState.userId = userId;
}

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setToken(state, action: PayloadAction<string>) {
      state.token = action.payload;
      state.isAuthenticated = true;
      
      const { role, userId } = decodeToken(action.payload);
      state.role = role;
      state.userId = userId;
      
      localStorage.setItem('token', action.payload);
    },
    removeToken(state) {
      state.token = null;
      state.isAuthenticated = false;
      state.role = null;
      state.userId = null;
      localStorage.removeItem('token');
    },
    checkTokenExpiration(state) {
      if (state.token) {
        try {
          const decoded = jwtDecode<JwtPayload>(state.token);
          if (decoded.exp * 1000 < Date.now()) {
            // Токен истёк
            state.token = null;
            state.isAuthenticated = false;
            state.role = null;
            state.userId = null;
            localStorage.removeItem('token');
          }
        } catch (error) {
          console.error('Ошибка проверки токена:', error);
        }
      }
    }
  },
});

export const { setToken, removeToken, checkTokenExpiration } = authSlice.actions;

export default authSlice.reducer;