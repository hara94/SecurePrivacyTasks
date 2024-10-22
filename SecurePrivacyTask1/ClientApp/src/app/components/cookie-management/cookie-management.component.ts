import { Component } from '@angular/core';
import { CookieConsentService } from '../../services/cookie-consent.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cookie-management',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cookie-management.component.html',
  styleUrls: ['./cookie-management.component.scss']
})
export class CookieManagementComponent {

  constructor(private cookieConsentService: CookieConsentService) { }

  // Clear non-essential cookies and revoke consent
  clearCookies(): void {
    this.cookieConsentService.clearNonEssentialCookies();
    this.cookieConsentService.setConsent(false); // Also revoke consent
  }
}
