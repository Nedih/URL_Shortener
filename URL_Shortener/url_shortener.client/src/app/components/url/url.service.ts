import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';


export interface URL {
  id: number;
  originalUrl: string;
  shortUrl: string;
  createdBy: string;
  createdDate: string;
}

@Injectable({
  providedIn: 'root',
})
export class UrlService {
  private apiUrl = environment.apiBaseUrl + "/auth"; 

  constructor(private http: HttpClient) { }

  // Get all URLs
  getUrls(): Observable<URL[]> {
    return this.http.get<URL[]>(this.apiUrl);
  }

  // Add a new URL
  addUrl(url: URL): Observable<URL> {
    return this.http.post<URL>(this.apiUrl, url);
  }

  // Delete a URL by ID
  deleteUrl(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Get details about a URL by ID
  getUrlById(id: number): Observable<URL> {
    return this.http.get<URL>(`${this.apiUrl}/${id}`);
  }
}
