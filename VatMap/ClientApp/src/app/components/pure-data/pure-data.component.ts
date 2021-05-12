import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/app/services/http.service';
import { Observable } from 'rxjs';
import { Snapshot } from 'src/app/model/snapshot';

@Component({
  selector: 'app-pure-data',
  templateUrl: './pure-data.component.html',
  styleUrls: ['./pure-data.component.css']
})
export class PureDataComponent implements OnInit {

  constructor(private http: HttpService) { }

  public snapshot: Snapshot | null = null;

  ngOnInit(): void {

  }

  public getSnapshot(): Observable<Snapshot> {
    return this.http.doGet("snapshot");
  }

  public dudla_click() {
    this.getSnapshot().subscribe(
      ret => this.snapshot = ret,
      err => console.log(err)
    );
  }
}
