import { Component} from '@angular/core';
import { AuthService } from './auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  imports: [FormsModule],
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService) { }

  onClickSubmit(result: { username: string; }) {
    console.log("You have entered : " + result.username);
  }

  login(): void {
    this.authService.login(this.email, this.password).subscribe({
      next: () => {
        alert('Login successful');
        localStorage.setItem('email', this.email); 
      },
      error: (err: { message: string; }) => {
        this.errorMessage = 'Login failed: ' + err.message;
      }
    });
  }
}
