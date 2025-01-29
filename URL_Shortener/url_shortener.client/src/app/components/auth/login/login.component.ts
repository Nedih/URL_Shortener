import { Component} from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { ToastService } from '../../../services/toast.service';

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

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router,
    private messageService: ToastService
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

    this.authService.logIn(email, password)
      .subscribe({
        next: (response: any) => {
          console.log('Login successful:', response);

          localStorage.setItem('token', response.accessToken);
          localStorage.setItem('userId', response.userId);
          localStorage.setItem('email', email);
          this.authService.authorize(response.roles);

          
        },
        error: (error) => {
          console.error('Login failed:', error);
          this.errorMessage = 'Invalid email or password.';
          this.isSubmitting = false;
        },
        complete: () => {
          this.messageService.showSuccess("You've been successfully logged in!");
          this.isSubmitting = false;
          this.router.navigate(['/']);
        }
      });
  }
}
