import { createSlice } from '@reduxjs/toolkit';

interface AuthState {
  token: string | null;
  isAuthenticated: boolean;
}

const initialState: AuthState = {
  token: localStorage.getItem('token') || null,
  isAuthenticated: !!localStorage.getItem('token'),
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setToken(state, action) {
      state.token = action.payload;
      state.isAuthenticated = true;
      localStorage.setItem('token', action.payload);
    },
    removeToken(state) {
      state.token = null;
      state.isAuthenticated = false;
      localStorage.removeItem('token');
    },
    
  },
});

export const { setToken, removeToken } = authSlice.actions;

export default authSlice.reducer;