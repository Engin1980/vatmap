import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private baseUrl = "http://localhost:55257/api/";

  constructor(private http: HttpClient) { }

  public doGet<T>(url: string): Observable<T> {
    const fullUrl = this.baseUrl + url;
    console.log("doing get to " + fullUrl);
    const ret$ = this.http.get<T>(fullUrl);
    return ret$;
  }
}
