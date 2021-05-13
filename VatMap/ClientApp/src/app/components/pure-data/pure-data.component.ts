import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/app/services/http.service';
import { Observable } from 'rxjs';
import { Snapshot } from 'src/app/model/snapshot';
import { Atc } from 'src/app/model/atc';

@Component({
  selector: 'app-pure-data',
  templateUrl: './pure-data.component.html',
  styleUrls: ['./pure-data.component.css']
})
export class PureDataComponent implements OnInit {

  constructor(private http: HttpService) { }

  public vm: Snapshot | null = null;

  ngOnInit(): void {

  }

  public getSnapshot(): Observable<Snapshot> {
    return this.http.doGet("snapshot");
  }

  public dudla_click() {
    this.vm = null;
    this.getSnapshot().subscribe(
      ret => {
        console.log(JSON.stringify(ret));
        this.vm = ret;
      },
      err => console.log(err)
    );
  }
}
