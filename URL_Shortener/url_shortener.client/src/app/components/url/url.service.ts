import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';


export interface URL {
  shortenUrl: string;
  urlCreationDate: string;
  urlDescription: string;
  urlText: string;
  userEmail: string;
  userId: string;
}

@Injectable({
  providedIn: 'root',
})
export class UrlService {
  private apiUrl = environment.apiBaseUrl + "/auth"; 

  constructor(private http: HttpClient) { }

  getUrls(): Observable<URL[]> {
    return this.http.get<URL[]>(this.apiUrl);
  }

  addUrl(url: URL): Observable<URL> {
    return this.http.post<URL>(this.apiUrl, url);
  }

  deleteUrl(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  createUrl(urlData: { urlText: string; urlDescription: string; userEmail: string }): Observable<any> {
    const { urlText, urlDescription, userEmail } = urlData;
    return this.http.post(`${environment.apiBaseUrl}/api/url/create`, { urlText, urlDescription, userEmail }, {
      headers: {
        'access-control-allow-origin': '*',
        'Content-Type': ['application/json', 'multipart/form-data'], 
      },
      withCredentials: true, 
    });
  }

  getUrlById(id: number): Observable<URL> {
    return this.http.get<URL>(`${this.apiUrl}/${id}`);
  }
}
