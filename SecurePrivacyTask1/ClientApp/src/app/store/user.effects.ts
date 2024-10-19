import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { User } from '../models/user';
import { UserService } from '../services/user.service';
import { loadUsers, loadUsersSuccess, createUser } from './user.actions';


@Injectable()
export class UserEffects {

  private actions$ = inject(Actions);
  private userService: UserService = inject(UserService);

  loadUsers$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadUsers),
      mergeMap(() => this.userService.getUsers().pipe(
        map((users: User[]) => loadUsersSuccess({ users })),
        catchError(() => of({ type: '[User List] Load Users Failed' }))
      ))
    )
  );

  createUser$ = createEffect(() =>
    this.actions$.pipe(
      ofType(createUser),
      mergeMap((action) => this.userService.createUser(action.user).pipe(
        map((user: User) => ({ type: '[User Create] Create User Success', user })),
        catchError(() => of({ type: '[User Create] Create User Failed' }))
      ))
    )
  );
}
