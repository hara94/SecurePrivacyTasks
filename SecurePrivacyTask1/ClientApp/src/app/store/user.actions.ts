import { createAction, props } from '@ngrx/store';
import { User } from '../models/user';

export const loadUsers = createAction('[User List] Load Users');
export const loadUsersSuccess = createAction('[User List] Load Users Success', props<{ users: User[] }>());
export const createUser = createAction('[User Create] Create User', props<{ user: User }>());
