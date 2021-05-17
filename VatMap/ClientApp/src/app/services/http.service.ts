import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LogService } from './log.service';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private baseUrl = "http://localhost:55257/api/";
  private log = new LogService("HttpService");

  constructor(private http: HttpClient) { }

  public get<T>(url: string): Observable<T> {
    const fullUrl = this.baseUrl + url;
    this.log.log("GET: " + fullUrl);
    const ret$ = this.http.get<T>(fullUrl);
    return ret$;
  }
}
