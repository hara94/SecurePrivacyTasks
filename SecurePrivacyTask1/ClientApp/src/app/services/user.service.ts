import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',  // Ensures that the service is tree-shakable and available globally
})
export class UserService {
  private apiUrl = 'https://localhost:7133/api/Users';

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl, { withCredentials: true });
  }

  getUserById(id: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`, { withCredentials: true });
  }

  createUser(user: User): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/create`, user, { withCredentials: true });
  }

  updateUser(user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${user.id}`, user, { withCredentials: true });  // Assuming 'id' is the unique identifier for the user
  }

  deleteUser(userId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${userId}`, { withCredentials: true });
  }

  // Fetch the profile of the current logged-in user based on their userId
  getUserProfile(): Observable<User> {
    const userId = this.getCurrentUserId();
    return this.http.get<User>(`${this.apiUrl}/${userId}`, { withCredentials: true });
  }

  // Get current user's ID from localStorage
  getCurrentUserId(): string | null {
    return localStorage.getItem('userId');
  }

  // Register a new user
  register(user: { userName: string, password: string, email: string, consentGiven: boolean }): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  // User login
  login(credentials: { userName: string, password: string }): Observable<{ success: boolean; message: string; userId: string }> {
    return this.http.post<{ success: boolean; message: string; userId: string }>(
      `${this.apiUrl}/login`,
      credentials,
      { withCredentials: true }
    );
  }

  // Check if the user is authenticated by making a simple request to a protected API route
  isAuthenticated(): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/is-authenticated`, { withCredentials: true });
  }

  // Logout the user
  logout(): Observable<any> {
    return this.http.post(`${this.apiUrl}/logout`, {}, { withCredentials: true });
  }

  // Clear cookie consent
  clearConsent(): void {
    localStorage.removeItem('cookieConsent');
  }

  // Save cookie consent to local storage
  saveConsent(consent: boolean): void {
    this.http.post('/api/save-consent', { consentGiven: consent }).subscribe();
  }

  // Get cookie consent from local storage
  getConsent(): boolean {
    return localStorage.getItem('cookieConsent') === 'true';
  }

  accessMyData(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/access-my-data`, { withCredentials: true });
  }
}
