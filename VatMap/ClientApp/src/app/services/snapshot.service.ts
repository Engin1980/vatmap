import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Snapshot } from '../model/snapshot';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class SnapshotService {

  private ROUTE = "snapshot/";

  constructor(private http: HttpService) { }

  public get(): Observable<Snapshot> {
    return this.http.get<Snapshot>(this.ROUTE);
  }
}
