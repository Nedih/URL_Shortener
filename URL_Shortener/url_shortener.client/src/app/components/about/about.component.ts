import { Component } from '@angular/core';
import { environment } from '../../../environments/environment';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { AuthService } from '../auth/auth.service';

@Component({
    selector: 'app-about',
    templateUrl: './about.component.html',
    styleUrl: './about.component.scss',
    imports: [FormsModule, NgIf]
})
export class AboutComponent {
  userText: string = environment.aboutPageText;
  isEditing: boolean = false;

  constructor(private authService: AuthService) { }

  toggleEdit(): void {
    this.isEditing = true;
  }

  saveEdit(): void {
    this.isEditing = false;
    console.log('Saved text:', this.userText);
  }

  cancelEdit(): void {
    this.isEditing = false;
  }

  isAdmin() {
    let res = this.authService.isAdmin();
    console.log("Is ADMIN? " + res);
    return res;
  }
}
