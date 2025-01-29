import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import $ from 'jquery';
import { UrlService } from '../../services/url.service';
import { LoadingService } from '../../services/loading-service';
import { MessageService } from 'primeng/api';
import { showError, showSuccess } from '../../utils/toast.util';
//import { MatCardModule } from '@angular/material/card';

interface Url {
  urlText: string;
  shortenUrl: string;
  urlCreationDate: string;
  urlDescription: string;
  userId: string;
  userEmail: string;
}

@Component({
  selector: 'app-url-table',
  templateUrl: './url.component.html',
  standalone: true,
  imports: [NgFor, NgIf, AsyncPipe],
  styleUrls: ['./url.component.scss'],
})
export class UrlTableComponent implements OnInit {
  public urls: Url[] = [];
  public selectedUrl: Url | null = null;
  //private _isLoading!: boolean;

  constructor(
    public loadingService: LoadingService,
    private http: HttpClient,
    private authService: AuthService,
    public urlService: UrlService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private messageService: MessageService
  ) { }

  ngOnInit() {
    //this._isLoading = this.loadingService.get();
    this.loadingService.show();
    this.changeDetectorRef.detectChanges();
    this.fetchUrls();
  }

  fetchUrls(){
    this.http.get<Url[]>(`${environment.apiBaseUrl}/api/url`, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        "access-control-allow-origin": "*",
        'Content-Type': ['application/json', 'multipart/form-data']
      },
      withCredentials: true
    }).subscribe({
      next: (result) => {
        console.log('Fetched URLs:', result);
        this.urls = result;
      },
      error: (error) => {
        console.error('Error fetching URLs:', error);
      },
      complete: () => {
        console.log('URL fetch complete');
        this.loadingService.hide();
        //this._isLoading = false;
        this.changeDetectorRef.detectChanges();
      }
    });
  }

  deleteUrl(shorten: string): void {
    this.http.delete(`${environment.apiBaseUrl}/api/url?shorten=${shorten}`, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        "access-control-allow-origin": "*",
        'Content-Type': ['application/json', 'multipart/form-data']
      },
      withCredentials: true }).subscribe({
      next: () => {
        console.log('Delete URL');
        showSuccess(this.messageService, "URL was deleted!");
      },
      error: (error) => {
        showError(this.messageService, error);
        console.error('Error deleting URLs:', error);
      },
      complete: () => {
        console.log('URL delete complete');
        this.fetchUrls();
      }
    });
  }

  addUrl() {
    this.router.navigate(['/url-add']);
  }

  isAdmin() {
    let res = this.authService.isAdmin();
    return res;
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  isUserOwner(email: string) {
    return this.authService.getEmail() === email;
  }

  viewUrlDetails(url: string): void {
    this.router.navigate(['/url-details', url]);
  }

  isLoading(): boolean {
    return this.loadingService.get();
  }

  title = 'url_shortener.client';
}
