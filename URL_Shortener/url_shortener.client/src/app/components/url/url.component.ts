import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { NgFor, NgIf } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import $ from 'jquery';

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
  imports: [NgFor, NgIf, RouterLink, RouterLinkActive],
  styleUrls: ['./url.component.scss'],
})
export class UrlTableComponent implements OnInit {
  public urls: Url[] = [];
  public selectedUrl: Url | null = null;

  constructor(private http: HttpClient, private authService: AuthService, private router: Router) { }

  ngOnInit() {
    /*this.isLoggedIn = this.authService.isLoggedIn();
    this.isAdmin = this.authService.isAdmin();
    this.userEmail = this.authService.getEmail();*/

    this.fetchUrls();
  }

  fetchUrls(){
    this.http.get<Url[]>(`${environment.apiBaseUrl}/api/url`).subscribe({
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

  deleteUrl(url: string): void {
    this.http.delete(`${environment.apiBaseUrl}/api/url`).subscribe({
      next: (result) => {
        console.log('Delete URL:', result);
      },
      error: (error) => {
        console.error('Error deleting URLs:', error);
      },
      complete: () => {
        console.log('URL delete complete');
      }
    });
  }

  isAdmin() {
    let res = this.authService.isAdmin();
    console.log(res);
    return res;
  }

  isUserOwner(email: string) {
    return this.authService.getEmail() === email;
  }

  viewUrlDetails(url: Url): void {
    this.router.navigate(['/url-details', url]);
  }

  title = 'url_shortener.client';
}
