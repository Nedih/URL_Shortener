import { Component } from '@angular/core';
import { environment } from '../../../environments/environment';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';

@Component({
    selector: 'app-about',
    templateUrl: './about.component.html',
    styleUrl: './about.component.scss',
  imports: [
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    CdkTextareaAutosize,
    NgIf
  ]
})
export class AboutComponent {
  userText: string = environment.aboutPageText;
  isEditing: boolean = false;
  fontSize = '16px';

  constructor(private authService: AuthService) { }

  toggleEdit(): void {
    if (this.isAdmin())
    this.isEditing = true;
  }

  saveEdit(): void {
    this.isEditing = false;
    console.log('Saved text:', this.userText);
  }

  cancelEdit(): void {
    this.isEditing = false;
  }

  triggerResize() {
    console.log("Font size changed to:", this.fontSize);
  }

  isAdmin() {
    let res = this.authService.isAdmin();
    console.log("Is ADMIN? " + res);
    return res;
  }
}
