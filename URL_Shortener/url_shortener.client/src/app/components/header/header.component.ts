import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { Router, RouterLink, RouterLinkActive} from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  imports: [ NgIf, RouterLink, RouterLinkActive],
  styleUrls: ['./header.component.css']
})
export class HeaderComponent{
  constructor(private authService: AuthService) { }

  isLoggedIn() : boolean {
    return this.authService.isLoggedIn();
  }

  onLogout(): void {
    this.authService.logout();
  }

  onLogin(): void {
    
  }
}

