import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink, RouterLinkActive} from '@angular/router';
import { NgIf } from '@angular/common';
import { MessageService } from 'primeng/api';
import { showSuccess } from '../../utils/toast.util';
//import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  imports: [NgIf, RouterLink, RouterLinkActive],
  styleUrls: ['./header.component.css']
})
export class HeaderComponent{
  clickedItem: any = null;
  constructor(private authService: AuthService, private messageService: MessageService) { }

  isLoggedIn() : boolean {
    return this.authService.isLoggedIn();
  }

  onLogout(): void {
    showSuccess(this.messageService, "You've been logged out!");
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

