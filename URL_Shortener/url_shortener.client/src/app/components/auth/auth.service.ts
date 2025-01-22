import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiBaseUrl}/api/auth`; 
  private roles: string[] = []; 
  private loggedIn = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient) { }

  login(roles: string[]): void
  {
    localStorage.setItem('roles', JSON.stringify(roles));
    this.loggedIn.next(true);
  }

  register(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}`, { email, password });
  }

  logout(): void {
    this.loggedIn.next(false);
    localStorage.removeItem('roles');
    localStorage.removeItem('email');
  }

  isLoggedIn(): boolean {
    return this.loggedIn.getValue() || !!localStorage.getItem('roles');
  }

  getRoles(): string[] {
    if (this.roles.length === 0) {
      const storedRoles = localStorage.getItem('roles');
      if (storedRoles) {
        this.roles = JSON.parse(storedRoles);
      }
    }
    return this.roles;
  }

  isAdmin(): boolean | undefined {
    let roles = localStorage.getItem('roles');
    if (roles === null)
      return false;
    else return roles.includes('Admin');
  }

  getEmail(): string | null {
    return localStorage.getItem('email');
  }

  getId(): string | null {
    return localStorage.getItem('userId');
  }
}
