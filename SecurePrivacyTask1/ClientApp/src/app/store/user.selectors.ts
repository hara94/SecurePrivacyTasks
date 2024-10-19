import { createFeatureSelector, createSelector } from '@ngrx/store';
import { User } from '../models/user';

export const selectUserState = createFeatureSelector<User[]>('users');

export const selectUsers = createSelector(
  selectUserState,
  (users: User[]) => users
);
