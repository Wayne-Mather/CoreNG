import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {EventBusService} from "./event-bus.service";
import {LoggerService} from "./logger.service";
import {RouterHelperService} from "./router-helper.service";

@NgModule({
  imports: [CommonModule],
  exports: [
    EventBusService,
    LoggerService,
    RouterHelperService
  ]
})
export class CorengCoreModule {}
