import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UrlService } from '../../../services/url.service'; 
import { NgIf } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { LoadingService } from '../../../services/loading.service';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-url-create',
  templateUrl: './url-create.component.html',
  standalone: true,
  imports: [NgIf, FormsModule, ReactiveFormsModule],
  styleUrls: ['./url-create.component.scss']
})
export class UrlCreateComponent {
  urlForm: FormGroup;
  errorMessage: string | null = null;
  isSubmitting = false;
  
  constructor(
    private fb: FormBuilder,
    private urlService: UrlService,
    private router: Router,
    private authService: AuthService,
    public loadingService: LoadingService,
    private messageService: ToastService
  )
  {
    this.urlForm = this.fb.group({
      urlText: ['', [Validators.required, Validators.pattern('https?://.+')]], 
      urlDescription: ['', [Validators.required, Validators.minLength(10)]] 
    }
    );
  }

  goBack(): void {
    window.history.back(); 
  }

  onSubmit(): void {
    if (this.urlForm.valid) {
      this.isSubmitting = true;
      this.errorMessage = null;

      const urlData = this.urlForm.value;
      const { urlText, urlDescription } = urlData;
      const userEmail = this.authService.getEmail() || '';

      this.urlService.createUrl({ urlText, urlDescription, userEmail }).subscribe({
        next: (response) => {
          this.messageService.showSuccess(`The URL has been added! Shorten URL is ${response.shortenUrl}`);
          this.router.navigate(['/']); 
        },
        error: (error) => {
          this.isSubmitting = false;
          this.errorMessage = error.error;
        }
      });
    }
  }
}
