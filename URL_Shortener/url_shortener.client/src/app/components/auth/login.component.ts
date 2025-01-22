import { Component} from '@angular/core';
import { AuthService } from './auth.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [ReactiveFormsModule],
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  email: string = '';
  password: string = '';
  loginForm: FormGroup;
  isSubmitting = false;
  errorMessage: string | null = null;

  constructor(private authService: AuthService, private fb: FormBuilder,
    private http: HttpClient,
    private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit(): void {
    this.errorMessage = null; 
    if (this.loginForm.invalid) {
      return;
    }

    this.isSubmitting = true;
    const { email, password } = this.loginForm.value;

    this.http.post(`${environment.apiBaseUrl}/api/auth/login`, { email, password })
      .subscribe({
        next: (response: any) => {
          console.log('Login successful:', response);

          localStorage.setItem('token', response.token);
          localStorage.setItem('userId', response.userId);
          localStorage.setItem('email', email);
          this.authService.login(response.roles);

          this.router.navigate(['/']);
        },
        error: (error) => {
          console.error('Login failed:', error);
          this.errorMessage = 'Invalid email or password.';
          this.isSubmitting = false;
        },
        complete: () => {
          
          this.isSubmitting = false;
        }
      });
  }
}
