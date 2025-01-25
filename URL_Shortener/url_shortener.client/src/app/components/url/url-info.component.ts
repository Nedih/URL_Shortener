import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink, RouterLinkActive } from '@angular/router';
import { environment } from '../../../environments/environment';
import { NgFor, NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
//import { MatTableModule } from '@angular/material/table';

interface Url {
  urlText: string;
  shortenUrl: string;
  urlCreationDate: string;
  urlDescription: string;
  userId: string;
  userEmail: string;
}

@Component({
  selector: 'app-url-details',
  templateUrl: './url-info.component.html',
  standalone: true,
  styleUrls: ['./url-info.component.scss']
})
export class UrlDetailsComponent implements OnInit {
  public selectedUrl: Url | undefined; 

  constructor(private route: ActivatedRoute, private http: HttpClient, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {

    const shortenUrl = this.route.snapshot.paramMap.get('shortenUrl') || '';

    //this.fetchUrl(shortenUrl);
    this.fetchUrls(shortenUrl);

    console.log(shortenUrl); 
  }

  fetchUrl(shortenUrl: string) {
    this.http.get<Url>(`${environment.apiBaseUrl}/api/url/${shortenUrl}`).subscribe({
      next: (result) => {
        console.log('Fetched URL:', result);
        this.selectedUrl = result;
      },
      error: (error) => {
        console.error('Error fetching URL:', error);
      },
      complete: () => {
        console.log('URL fetch complete');
      }
    });
  }

  fetchUrls(shortenUrl: string) {
    this.http.get<Url[]>(`${environment.apiBaseUrl}/api/url`).subscribe({
      next: (result) => {
        console.log('Fetched URLs:', result);
        this.selectedUrl = result.find(x => x.shortenUrl = shortenUrl);
      },
      error: (error) => {
        console.error('Error fetching URLs:', error);
      },
      complete: () => {
        console.log('URL fetch complete');
      }
    });
  }

  goBack() {
    this.router.navigate(['/']);
  }
}
