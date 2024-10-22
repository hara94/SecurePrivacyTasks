import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { User } from '../../models/user';
import { selectUsers } from '../../store/user.selectors';
import { deleteUser, loadUsers } from '../../store/user.actions';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  private store = inject(Store);
  private userService = inject(UserService);
  private router = inject(Router);

  users$: Observable<User[]> = this.store.select(selectUsers);
  currentUser: User | null = null;

  ngOnInit(): void {
    this.store.dispatch(loadUsers());

    // Fetch current user profile to check permissions
    this.userService.getUserProfile().subscribe(user => {
      this.currentUser = user;
    });
  }

  onDeleteUser(userId: string): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.store.dispatch(deleteUser({ userId }));
    }
  }

  onUpdateUser(user: User): void {
    // Navigate to the edit route and pass the user ID
    this.router.navigate(['/edit-user', user.id]);
  }
}
