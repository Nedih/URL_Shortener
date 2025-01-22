/*import { Component } from '@angular/core';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  constructor(private authService: AuthService) { }

  get isAuthenticated(): boolean {
    return this.authService.isLoggedIn();
  }

  logout(): void {
    this.authService.logout();
  }
}*/
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { Router, RouterLink, RouterLinkActive} from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  imports: [/*NgIf,*/ RouterLink, RouterLinkActive],
  styleUrls: ['./header.component.css']
})
export class HeaderComponent{
  isLoggedIn = false;
  isAdmin = false;

  constructor(private authService: AuthService) { }

  onLogout(): void {
    this.authService.logout();
  }

  onLogin(): void {
  }
}

