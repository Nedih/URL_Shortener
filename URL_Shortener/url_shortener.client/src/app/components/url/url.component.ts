import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { NgFor, NgIf } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface Url {
  urlText: string;
  shortenUrl: string;
  urlCreationDate: string;
  urlDescription: string;
  userId: string;
}

@Component({
  selector: 'app-url-table',
  templateUrl: './url.component.html',
  standalone: true,
  imports: [NgFor, NgIf, RouterLink, RouterLinkActive],
  styleUrls: ['./url.component.css'],
})
export class UrlTableComponent implements OnInit {
  public urls: Url[] = [];

  constructor(private http: HttpClient) { }

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

  deleteUrl(id: number): void {
    
    
  }
  title = 'url_shortener.client';
}
