import { Component} from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { NgIf } from '@angular/common';
import { MessageService } from 'primeng/api';
import { showSuccess } from '../../utils/toast.util';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [ NgIf, FormsModule, ReactiveFormsModule],
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  email: string = '';
  password: string = '';
  loginForm: FormGroup;
  isSubmitting = false;
  errorMessage: string | null = null;

  constructor(private authService: AuthService,
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private messageService: MessageService
  ) {
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

    this.http.post(`${environment.apiBaseUrl}/api/auth/login`, { email, password }, {
      headers: {
        "access-control-allow-origin": "*",
        'Content-Type': ['application/json', 'multipart/form-data']
      },
      withCredentials: true
    })
      .subscribe({
        next: (response: any) => {
          console.log('Login successful:', response);

          localStorage.setItem('token', response.accessToken);
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
          showSuccess(this.messageService, "You've been successfully logged in!");
          this.isSubmitting = false;
        }
      });
  }
}
