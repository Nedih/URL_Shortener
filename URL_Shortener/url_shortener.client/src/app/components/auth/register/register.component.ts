import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { ToastService } from '../../../services/toast.service';

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
    private authService: AuthService,
    private messageService: ToastService
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

    this.authService.register(formValues).subscribe({
      next: () => {
      },
      error: (err: { message: string; }) => {
        this.errorMessage = 'Registration failed: ' + err.message;
        this.registerForm.reset(); 
        this.submitted = false;
        this.isSubmitting = false;
      },
      complete: () => {
        this.messageService.showSuccess(`You've been sucessfully registered, ${formValues.email}!`);
        this.isSubmitting = false;
        this.router.navigate(['/login']);
      }
    });
  }
}
