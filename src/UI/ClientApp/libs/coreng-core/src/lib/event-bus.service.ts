import { Injectable } from '@angular/core';
import { Subject, Subscription, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import {BusEvents} from "./events.enum";

@Injectable({
  providedIn: 'root'
})
export class EventBusService {

  private subject = new Subject<any>();

  constructor() { }

  on(event: BusEvents, action: any): Subscription {
    return this.subject
      .pipe(
        filter((e: EmitEvent) => {
          return e.name === event;
        }),
        map((event: EmitEvent) => {
          return event.value;
        })
      )
      .subscribe(action);
  }

  emit(name: BusEvents, value?: any) {
    this.subject.next(new EmitEvent(name, value));
  }

  // emit(event: EmitEvent) {
  //   this.subject.next(event);
  // }
}

export class EmitEvent {

  constructor(public name: any, public value?: any) { }

}

