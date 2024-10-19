import { Routes } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserCreateComponent } from './components/user-create/user-create.component';

// Define the routes for the application
export const routes: Routes = [
  { path: '', component: UserListComponent },  // Default route to list users
  { path: 'create', component: UserCreateComponent },  // Route to create a user
  { path: '**', redirectTo: '' }  // Wildcard route to redirect any unknown paths to the user list
];
