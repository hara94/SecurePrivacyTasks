import { Injectable, inject } from "@angular/core";
import { Router } from "@angular/router";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { tap } from "rxjs";
import { loginSuccess, logout } from "./auth.actions";

@Injectable()
export class AuthEffects {
  private actions$ = inject(Actions);
  private router: Router = inject(Router);

  // Effect to handle successful login
  loginSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loginSuccess),
      tap(({ userId }) => {
        if (userId) {
          localStorage.setItem('userId', userId);
        }
      })
    ),
    { dispatch: false }  
  );

  // Effect to handle logout
  logout$ = createEffect(() =>
    this.actions$.pipe(
      ofType(logout),
      tap(() => {
        localStorage.removeItem('userId'); 
        this.router.navigate(['/login']);  
      })
    ),
    { dispatch: false } 
  );
}
