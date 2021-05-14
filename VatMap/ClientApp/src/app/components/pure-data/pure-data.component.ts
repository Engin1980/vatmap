import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/app/services/http.service';
import { Observable } from 'rxjs';
import { Snapshot } from 'src/app/model/snapshot';
import { SnapshotService } from 'src/app/services/snapshot.service';

@Component({
  selector: 'app-pure-data',
  templateUrl: './pure-data.component.html',
  styleUrls: ['./pure-data.component.css']
})
export class PureDataComponent implements OnInit {

  constructor(private snapshotService: SnapshotService) { }

  public vm: Snapshot | null = null;

  ngOnInit(): void {

  }

  public dudla_click() {
    this.vm = null;
    this.snapshotService.get().subscribe(
      ret => {
        console.log(JSON.stringify(ret));
        this.vm = ret;
      },
      err => console.log(err)
    );
  }
}
