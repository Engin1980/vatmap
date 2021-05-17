import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LogService {

  private parent: string;
  constructor(parent: string) {
    this.parent = parent;
  }

  public log(msg: string): void {
    console.log("#log# :: " + parent + " :: " + msg);
  }
}


