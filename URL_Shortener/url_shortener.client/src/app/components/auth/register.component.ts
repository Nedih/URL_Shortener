import { Component } from '@angular/core';
import { AuthService } from './auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true, 
  templateUrl: './register.component.html',
  imports: [FormsModule],
  styleUrls: ['./register.component.css']
})
export class RegisterComponent{
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  errorMessage: string = '';

  onClickSubmit(result: { username: string; }) {
    console.log("You have entered : " + result.username);
  }

  constructor(private authService: AuthService) { }

  register(): void {
    if (this.password !== this.confirmPassword) {
      this.errorMessage = "Passwords do not match!";
      return;
    }

    this.authService.register(this.email, this.password).subscribe({
      next: () => {
        alert('Registration successful');
      },
      error: (err) => {
        this.errorMessage = 'Registration failed: ' + err.message;
      }
    });
  }
}
