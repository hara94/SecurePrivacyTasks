import { createReducer, on } from '@ngrx/store';
import { loadUsersSuccess, createUser } from './user.actions';
import { User } from '../models/user';

export const initialState: User[] = [];

export const userReducer = createReducer(
  initialState,
  on(loadUsersSuccess, (state, { users }) => [...users]),
  on(createUser, (state, { user }) => [...state, user])
);
