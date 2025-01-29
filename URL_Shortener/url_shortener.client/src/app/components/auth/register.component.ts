import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators, FormControl, AbstractControl, ValidatorFn } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { showSuccess } from '../../utils/toast.util';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-register',
  standalone: true, 
  templateUrl: './register.component.html',
  imports: [FormsModule, ReactiveFormsModule, NgIf],
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  errorMessage: string | null = null;
  registerForm: FormGroup;
  submitted = false;
  isSubmitting = false;

  constructor(private fb: FormBuilder,
    private router: Router,
    private http: HttpClient,
    private messageService: MessageService
  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
    }, {
      validators: this.mustMatch('password', 'confirmPassword')
    });
  }

  get f() {
    return this.registerForm.controls;
  }

  mustMatch(controlName: string, matchingControlName: string): ValidatorFn {
    return (group: AbstractControl): { [key: string]: boolean } | null => {
      const control = group.get(controlName);
      const matchingControl = group.get(matchingControlName);
      if (matchingControl?.errors && !matchingControl?.errors['mustMatch']) {
        return null;
      }
      if (control?.value !== matchingControl?.value) {
        matchingControl?.setErrors({ mustMatch: true });
      } else {
        matchingControl?.setErrors(null);
      }
      return null;
    };
  }

  onSubmit(): void {
    this.submitted = true;
    this.errorMessage = null;

    if (this.registerForm.invalid) {
      console.log('Form is invalid');

      return;
    }

    this.isSubmitting = true;

    const formValues = this.registerForm.value;
    console.log('Registration data:', formValues);

    this.http.post(`${environment.apiBaseUrl}/api/auth/register`, { formValues }).subscribe({
      next: () => {
        alert('Registration successful');
      },
      error: (err: { message: string; }) => {
        this.errorMessage = 'Registration failed: ' + err.message;
        this.registerForm.reset(); 
        this.submitted = false;
        this.isSubmitting = false;
      },
      complete: () => {
        showSuccess(this.messageService, `You've been sucessfully registered, ${formValues.email}!`);
        this.isSubmitting = false;
        this.router.navigate(['/login']);
      }
    });
  }
}
