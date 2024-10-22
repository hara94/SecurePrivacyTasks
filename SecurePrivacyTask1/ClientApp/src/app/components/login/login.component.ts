import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { loginSuccess } from '../../store/auth.actions';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [FormsModule, CommonModule],
})
export class LoginComponent {
  credentials = { userName: '', password: '' };
  login$: Observable<{ success: boolean; message: string; userId: string }> | undefined;
  loggedInMessage: string = '';  // For showing success message

  constructor(private userService: UserService, private router: Router, private store: Store) { }

  ngOnInit(): void {
    // Check if already logged in and redirect to /users if authenticated
    this.userService.isAuthenticated().subscribe(isAuthenticated => {
      if (isAuthenticated) {
        this.router.navigate(['/users']);
      }
    });
  }

  onSubmit(): void {
    // Assign the login observable but don't subscribe yet
    this.login$ = this.userService.login(this.credentials).pipe(
      tap(response => {
        if (response.success) {
          // Dispatch the loginSuccess action with the userId
          this.store.dispatch(loginSuccess({ userId: response.userId }));
          // Display a success message briefly and redirect to /users
          this.loggedInMessage = 'Logged in successfully. Navigating to users site...';
          setTimeout(() => {
            this.router.navigate(['/users']);  // Navigate to users after login
          }, 1000);  // Small delay to show message
        } else {
          console.error('Login failed:', response.message || 'Unknown error');
        }
      })
    );

    // Subscribe to the observable separately
    this.login$.subscribe();
  }
}
