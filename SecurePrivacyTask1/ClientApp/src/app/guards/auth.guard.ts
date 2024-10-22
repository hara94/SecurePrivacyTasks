// auth.guard.ts
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { UserService } from '../services/user.service';
import { loginSuccess, logout } from '../store/auth.actions';
import { AuthState } from '../store/auth.reducer';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private userService: UserService,
    private store: Store<AuthState>,
    private router: Router
  ) { }

  canActivate(): Observable<boolean> {
    return this.userService.isAuthenticated().pipe(
      map((isAuthenticated: boolean) => {
        if (isAuthenticated) {
          // If authenticated, dispatch login success and continue
          const userId = localStorage.getItem('userId'); // Retrieve userId from local storage
          if (userId) {
            this.store.dispatch(loginSuccess({ userId }));
          }
          return true;
        } else {
          // If not authenticated, dispatch logout and redirect to login
          this.store.dispatch(logout());
          this.router.navigate(['/login']);
          return false;
        }
      }),
      catchError(() => {
        // Handle errors and redirect to login
        this.store.dispatch(logout());
        this.router.navigate(['/login']);
        return of(false);
      })
    );
  }
}
