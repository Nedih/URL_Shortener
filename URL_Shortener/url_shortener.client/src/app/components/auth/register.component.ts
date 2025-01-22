import { Component } from '@angular/core';
import { AuthService } from './auth.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-register',
  standalone: true, 
  templateUrl: './register.component.html',
  imports: [ReactiveFormsModule],
  styleUrls: ['./register.component.css']
})
export class RegisterComponent{
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  errorMessage: string = '';
  registerForm: FormGroup;
  submitted = false;
    http: any;

  onClickSubmit(result: { username: string; }) {
    console.log("You have entered : " + result.username);
  }

  constructor(private fb: FormBuilder, private router: Router) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
    });
  }
  get f() {
    return this.registerForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.registerForm.invalid) {
      console.log('Form is invalid');
      return;
    }

    const formValues = this.registerForm.value;
    console.log('Registration data:', formValues);

    this.http.post(`${environment.apiBaseUrl}/api/auth/register`, { formValues }).subscribe({
      next: () => {
        alert('Registration successful');
      },
      error: (err: { message: string; }) => {
        this.errorMessage = 'Registration failed: ' + err.message;
      }
    });
    alert('Registration successful!');
    this.router.navigate(['/login']); // Redirect to login after successful registration


  }
}



function register(email: string, password: string) {
    throw new Error('Function not implemented.');
}
