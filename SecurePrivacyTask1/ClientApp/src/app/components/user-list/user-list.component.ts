import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { User } from '../../models/user';
import { selectUsers } from '../../store/user.selectors';
import { loadUsers } from '../../store/user.actions';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  private store = inject(Store);

  users$: Observable<User[]> = this.store.select(selectUsers);


  ngOnInit(): void {
    this.store.dispatch(loadUsers());
  }
}
