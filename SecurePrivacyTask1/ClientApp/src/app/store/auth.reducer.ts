import { createReducer, on } from '@ngrx/store';
import { loginSuccess, logout } from './auth.actions';

export interface AuthState {
  userId: string | null;
  isAuthenticated: boolean;
}

export const initialState: AuthState = {
  userId: null,
  isAuthenticated: false,
};

// Load userId from localStorage when the app starts
const userIdFromStorage = localStorage.getItem('userId');
const initialAuthState: AuthState = {
  userId: userIdFromStorage,
  isAuthenticated: !!userIdFromStorage,
};

export const authReducer = createReducer(
  initialAuthState,
  on(loginSuccess, (state, { userId }) => ({
    ...state,
    userId: userId,
    isAuthenticated: true
  })),
  on(logout, (state) => ({
    ...state,
    userId: null,
    isAuthenticated: false
  }))
);
