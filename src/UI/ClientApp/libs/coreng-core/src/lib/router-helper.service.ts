import { Injectable } from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class RouterHelperService {

  constructor() { }

  public getParameter(route:ActivatedRoute, name: string): string {
    if(route) {
      for(let propt in route.snapshot.params) {
        if(propt == name) {
          return route.snapshot.params[propt];
        }
      }
    }
    return '';
  }
}
