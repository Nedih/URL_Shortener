import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';

export interface registerModel {
  email: string,
  name: string,
  password: string,
  confirmPassword: string
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiBaseUrl}/api/auth`; 
  private roles: string[] = []; 
  private loggedIn = new BehaviorSubject<boolean>(false);

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  authorize(roles: string[]): void
  {
    localStorage.setItem('roles', JSON.stringify(roles));
    this.loggedIn.next(true);
  }

  logIn(email: string, password: string) {
    return this.http.post(`${this.apiUrl}/login`, { email, password }, {
      headers: {
        "access-control-allow-origin": "*",
        'Content-Type': ['application/json', 'multipart/form-data']
      },
      withCredentials: true
    })
  }

  register(user: registerModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, { user }, {
      headers: {
        "access-control-allow-origin": "*",
        'Content-Type': ['application/json', 'multipart/form-data']
      },
      withCredentials: true
    });
  }

  logOut(): void {
    this.loggedIn.next(false);
    localStorage.removeItem('roles');
    localStorage.removeItem('email');
    localStorage.removeItem('token');  
    this.router.navigate(['/']);
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
