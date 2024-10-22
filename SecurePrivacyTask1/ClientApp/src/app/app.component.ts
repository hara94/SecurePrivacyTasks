import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { UserService } from './services/user.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CookieBannerComponent } from './components/cookie-banner/cookie-banner.component';
import { CookieManagementComponent } from './components/cookie-management/cookie-management.component';
import { PrivacyPolicyComponent } from './components/privacy-policy/privacy-policy.component';
import { selectIsAuthenticated } from './store/auth.selectors';
import { logout } from './store/auth.actions';
import { CookieConsentService } from './services/cookie-consent.service'; // Import the CookieConsentService

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [CookieBannerComponent, CookieManagementComponent, PrivacyPolicyComponent, RouterModule, CommonModule],
})
export class AppComponent {
  title = 'User Management App';
  authenticated$: Observable<boolean>;

  constructor(
    private store: Store,
    private userService: UserService,
    private router: Router,
    private cookieConsentService: CookieConsentService // Inject CookieConsentService
  ) {
    // Use selector to get the authenticated state from the store
    this.authenticated$ = this.store.select(selectIsAuthenticated);
  }

  ngOnInit(): void {
    // Listen for changes in authentication status
    this.authenticated$.subscribe(isAuthenticated => {
      if (!isAuthenticated) {
        this.router.navigate(['/login']);
      }
    });
  }

  // Check if the user has consented to cookies
  hasConsented(): boolean {
    return this.cookieConsentService.hasConsented();
  }

  // Handle logout
  logout(): void {
    this.userService.logout().subscribe(() => {
      // Dispatch logout action
      this.store.dispatch(logout());
      this.router.navigate(['/login']);
    });
  }
}
