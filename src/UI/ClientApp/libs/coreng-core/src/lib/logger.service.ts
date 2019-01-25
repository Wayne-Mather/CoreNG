import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoggerService {

  public showMessages: boolean = true;

  constructor() { }

  log(name: string, msg: string, data?:any) {
    if(this.showMessages) {
      if(data) {
        console.log(name + ': ' + msg, data);
      }
      else {
        console.log(name + ': ' + msg, data);
      }
    }
  }
}
