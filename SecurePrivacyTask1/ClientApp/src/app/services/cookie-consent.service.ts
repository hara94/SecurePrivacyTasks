import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CookieConsentService {
  private consentKey = 'cookieConsent';

  // Check if user has already consented to cookies
  hasConsented(): boolean {
    return localStorage.getItem(this.consentKey) === 'true';
  }

  // Set consent in local storage
  setConsent(consent: boolean): void {
    localStorage.setItem(this.consentKey, String(consent));
    if (!consent) {
      this.clearNonEssentialCookies();
    }
  }

  // Clear non-essential cookies
  clearNonEssentialCookies(): void {
    // Clear any cookies that are non-essential (example cookies for demonstration)
    this.deleteCookie('analytics');
    this.deleteCookie('ad-preferences');
  }

  // Set non-essential cookies (e.g., for analytics)
  setNonEssentialCookies(): void {
    this.setCookie('analytics', 'enabled', 365); // Example: Set an analytics cookie for 1 year
    this.setCookie('ad-preferences', 'enabled', 365); // Example: Set ad-preference cookie
  }

  // Set a cookie with name, value, and expiry days
  private setCookie(name: string, value: string, days: number): void {
    const date = new Date();
    date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
    const expires = 'expires=' + date.toUTCString();
    document.cookie = `${name}=${value}; ${expires}; path=/`;
  }

  // Delete a cookie by name
  private deleteCookie(name: string): void {
    document.cookie = `${name}=; Max-Age=0; path=/;`;
  }
}
