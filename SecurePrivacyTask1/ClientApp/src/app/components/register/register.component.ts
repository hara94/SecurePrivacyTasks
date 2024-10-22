import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';  // Import ToastrService
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  imports: [FormsModule, CommonModule, RouterModule]
})
export class RegisterComponent {
  user = { userName: '', password: '', email: '', consentGiven: false };
  errorMessage: string = '';  // To store error messages

  constructor(
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService  // Inject ToastrService
  ) { }

  onSubmit(): void {
    if (this.user.consentGiven) {
      this.userService.register(this.user).pipe(
        catchError(error => {
          // Handle error, show toaster notifications
          if (error.status === 400 && error.error === 'Username is already taken') {
            this.toastr.error('Username is already taken. Please choose another one.', 'Registration Failed');
          } else {
            this.toastr.error('An unexpected error occurred. Please try again.', 'Error');
          }
          return of(null);  // Prevent observable from breaking
        })
      ).subscribe(response => {
        if (response) {
          // Show success toaster on registration success
          this.toastr.success('User registered successfully.', 'Success');
          this.router.navigate(['/login']);  // Navigate to login after success
        }
      });
    } else {
      this.toastr.warning('You must agree to the terms and conditions.', 'Warning');
    }
  }
}
