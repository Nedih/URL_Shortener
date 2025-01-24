import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UrlService } from './url.service'; // Make sure to create this service
import { NgIf } from '@angular/common';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

interface UrlResponse {
  shortenUrl: string;
  // other properties if needed
}

@Component({
  selector: 'app-url-create',
  templateUrl: './url-add.component.html',
  standalone: true,
  imports: [NgIf, FormsModule, ReactiveFormsModule],
  styleUrls: ['./url-add.component.scss']
})
export class UrlCreateComponent implements OnInit {
  urlForm: FormGroup;
  errorMessage: string | null = null;
  isSubmitting = false;
  
  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    //private urlService: UrlService,
    private router: Router,
    private authService: AuthService
    
  )
  {
    this.urlForm = this.fb.group({
      urlText: ['', [Validators.required, Validators.pattern('https?://.+')]], 
      urlDescription: ['', [Validators.required, Validators.minLength(10)]] 
    }
    );
  }

  ngOnInit(): void {
    
  }

  onSubmit(): void {
    if (this.urlForm.valid) {
      this.isSubmitting = true;
      this.errorMessage = null;

      const urlData = this.urlForm.value;
      const { urlText, urlDescription } = urlData;
      const userEmail = this.authService.getEmail() || '';
      const token = localStorage.getItem('token');

      this.http.post<UrlResponse>(`${environment.apiBaseUrl}/api/url/create`, { urlText, urlDescription, userEmail }, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'access-control-allow-origin': '*',
          'Content-Type': ['application/json', 'multipart/form-data'],
        },
        withCredentials: true,
      }).subscribe({
        next: (response) => {
          alert(`Shorten URL is ${response.shortenUrl}`);
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
