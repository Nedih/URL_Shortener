import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { NgFor, NgIf } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import $ from 'jquery';
import { UrlService } from '../../services/url.service';
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
  imports: [NgFor, NgIf],
  styleUrls: ['./url.component.scss'],
})
export class UrlTableComponent implements OnInit {
  public urls: Url[] = [];
  public selectedUrl: Url | null = null;

  constructor(private http: HttpClient, private authService: AuthService, public urlService: UrlService, private router: Router) { }

  ngOnInit() { this.fetchUrls();}

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
      },
      error: (error) => {
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
    console.log("Is ADMIN? " + res);
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

  title = 'url_shortener.client';
}
