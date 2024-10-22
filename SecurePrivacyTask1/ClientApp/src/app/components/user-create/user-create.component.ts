import { Component, OnInit, inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Component({
  selector: 'app-user-create',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.scss'],
})
export class UserCreateComponent implements OnInit {
  user: User = {   // Initialize user data
    userName: '',
    email: '',
    phone: '',
    address: '',
    city: '',
    consentGiven: false,
    canCreateUsers: false,
    canDeleteUsers: false,
    canEditUsers: false,
  };

  isEditMode: boolean = false;  // Default to create mode
  currentUser: User | null = null;
  private userService = inject(UserService);
  private route = inject(ActivatedRoute);

  constructor(private toastr: ToastrService) { }

  ngOnInit(): void {
    // Fetch the current user's profile and permissions
    this.userService.getUserProfile().subscribe({
      next: (user) => {
        this.currentUser = user;
      },
      error: () => {
        this.toastr.error('Failed to load current user profile.', 'Error');
      }
    });

    // Check if we have a user ID in the route (edit mode)
    const userId = this.route.snapshot.paramMap.get('id');
    if (userId) {
      this.isEditMode = true;
      this.loadUser(userId);  // Load the user data if in edit mode
    }
  }

  loadUser(userId: string): void {
    this.userService.getUserById(userId).subscribe({
      next: (user) => {
        this.user = user;  // Populate the form with user data
      },
      error: () => {
        this.toastr.error('Failed to load user data.', 'Error');
      }
    });
  }

  onSubmit(): void {
    if (!this.user.consentGiven) {
      this.toastr.error('Please give consent to store your personal data.', 'Consent Required');
      return;
    }

    if (this.isEditMode) {
      if (this.currentUser?.canEditUsers) {
        this.updateUser();
      } else {
        this.toastr.error('You do not have permission to update users.', 'Permission Denied');
      }
    } else {
      this.createUser();
    }
  }

  createUser(): void {
    this.userService
      .createUser(this.user)
      .pipe(
        tap(() => {
          this.toastr.success('User created successfully!', 'Success');
        }),
        catchError((error) => {
          this.handleError(error);
          return of(null);
        })
      )
      .subscribe();
  }

  updateUser(): void {
    this.userService
      .updateUser(this.user)
      .pipe(
        tap(() => {
          this.toastr.success('User updated successfully!', 'Success');
        }),
        catchError((error) => {
          this.handleError(error);
          return of(null);
        })
      )
      .subscribe();
  }

  handleError(error: any): void {
    this.toastr.error('An error occurred. Please try again.', 'Error');
  }
}
