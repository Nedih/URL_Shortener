import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ToastService } from './toast.service';

export interface Url {
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
  private apiUrl = environment.apiBaseUrl + "/api/url"; 

  constructor(
    private http: HttpClient,
    private messageService: ToastService
  ) { }

  getUrls(): Observable<Url[]> {
    return this.http.get<Url[]>(this.apiUrl, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        "access-control-allow-origin": "*",
        'Content-Type': ['application/json', 'multipart/form-data']
      },
      withCredentials: true
    });
  }

  addUrl(url: Url): Observable<Url> {
    return this.http.post<Url>(this.apiUrl, url);
  }

  deleteUrl(shorten: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}?shorten=${shorten}`, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        "access-control-allow-origin": "*",
        'Content-Type': ['application/json', 'multipart/form-data']
      },
      withCredentials: true
    });
  }

  createUrl(urlData: { urlText: string; urlDescription: string; userEmail: string }): Observable<any> {
    const { urlText, urlDescription, userEmail } = urlData;
    return this.http.post(`${this.apiUrl}/create`, { urlText, urlDescription, userEmail }, {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        'access-control-allow-origin': '*',
        'Content-Type': ['application/json', 'multipart/form-data'], 
      },
      withCredentials: true, 
    });
  }

  getUrl(shortenUrl: string): Observable<Url> {
    return this.http.get<Url>(`${this.apiUrl}/${shortenUrl}`);
  }

  copyText(text: string | undefined) {
    if (text)
      navigator.clipboard.writeText(text)
        .then(() => {
          this.messageService.showSuccess('Text copied to clipboard!');
        })
        .catch(err => {
          this.messageService.showError(`Failed to copy text: ${err}`);
        });
  }
}
