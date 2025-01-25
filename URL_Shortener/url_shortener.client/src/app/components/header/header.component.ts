import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink, RouterLinkActive} from '@angular/router';
import { NgIf } from '@angular/common';
//import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  imports: [NgIf, RouterLink, RouterLinkActive],
  styleUrls: ['./header.component.css']
})
export class HeaderComponent{
  clickedItem: any = null;
  constructor(private authService: AuthService) { }

  isLoggedIn() : boolean {
    return this.authService.isLoggedIn();
  }

  onLogout(): void {
    this.authService.logout();
  }

  onLogin(): void {
    
  }

  onClick(event: Event) {
    const li = event.target as HTMLElement;

    if (this.clickedItem) {
      this.clickedItem.classList.remove('clicked');
    }

    li.classList.add('clicked');
    this.clickedItem = li; 
  }
}

