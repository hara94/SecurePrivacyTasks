import { Component } from '@angular/core';
import { CookieConsentService } from '../../services/cookie-consent.service';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-cookie-banner',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cookie-banner.component.html',
  styleUrls: ['./cookie-banner.component.scss']
})
export class CookieBannerComponent {

  constructor(
    private cookieConsentService: CookieConsentService,
    private userService: UserService
  ) { }

  hasConsented(): boolean {
    return this.cookieConsentService.hasConsented();
  }

  // Accept cookies and save consent both locally and on the server
  acceptCookies(): void {
    this.cookieConsentService.setConsent(true);
    this.cookieConsentService.setNonEssentialCookies(); // Set non-essential cookies
    this.userService.saveConsent(true);  // Optionally save consent to backend
  }

  // Decline cookies and clear non-essential cookies locally
  declineCookies(): void {
    this.cookieConsentService.setConsent(false);
    this.cookieConsentService.clearNonEssentialCookies();
    this.userService.saveConsent(false);  // Optionally save consent to backend
  }
}
