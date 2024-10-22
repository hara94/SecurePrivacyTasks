import { Routes } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserCreateComponent } from './components/user-create/user-create.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth.guard';
import { CookieManagementComponent } from './components/cookie-management/cookie-management.component';
import { PrivacyPolicyComponent } from './components/privacy-policy/privacy-policy.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';

export const routes: Routes = [
  // Public routes
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'cookie-management', component: CookieManagementComponent },
  { path: 'privacy-policy', component: PrivacyPolicyComponent },

  // Protected routes
  { path: 'users', component: UserListComponent, canActivate: [AuthGuard] },
  { path: 'create', component: UserCreateComponent, canActivate: [AuthGuard] },
  { path: 'edit-user/:id', component: UserCreateComponent, canActivate: [AuthGuard] },
  { path: 'profile', component: UserProfileComponent, canActivate: [AuthGuard] },

  // Default and fallback routes
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];
