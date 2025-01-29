import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { RouterLink, RouterLinkActive} from '@angular/router';
import { NgIf } from '@angular/common';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  imports: [NgIf, RouterLink, RouterLinkActive],
  styleUrls: ['./header.component.css']
})
export class HeaderComponent{
  clickedItem: any = null;
  constructor(
    private authService: AuthService,
    private messageService: ToastService
  ) { }

  isLoggedIn() : boolean {
    return this.authService.isLoggedIn();
  }

  onLogout(): void {
    this.messageService.showSuccess("You've been logged out!");
    this.authService.logOut();
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
