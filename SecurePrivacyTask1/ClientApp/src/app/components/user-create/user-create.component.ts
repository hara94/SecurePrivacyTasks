import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';  // Import ToastrService
import { of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Component({
  selector: 'app-user-create',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.scss']
})
export class UserCreateComponent {
  user: User = { userName: '', email: '', phone: '', address: '', city: '', consentGiven: false };

  constructor(private userService: UserService, private toastr: ToastrService) { }  // Inject ToastrService

  onSubmit(): void {
    if (!this.user.consentGiven) {
      // Show error toast notification if consent is not given
      this.toastr.error('Please give consent to store your personal data.', 'Consent Required');
      return;
    }

    this.userService.createUser(this.user)
      .pipe(
        tap(() => {
          this.toastr.success('User created successfully!', 'Success');
        }),
        catchError((error) => {
          if (error.status === 400) {
            this.toastr.error('User creation failed. Please check your input.', 'Error');
          } else {
            this.toastr.error('An unexpected error occurred. Please try again later.', 'Error');
          }
          return of(null);
        })
      )
      .subscribe();
  }
}
