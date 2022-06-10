import { Injectable } from '@angular/core';
import { Subject, Subscriber } from 'rxjs';
import { SignalrService } from './signalr.service';

export interface PieChartData {
  labels: string[];
  values: number[];
}

@Injectable({
  providedIn: 'root'
})
export class PieChartService {

  data$ = new Subject<PieChartData>();

  constructor(private signalRService: SignalrService) {

    if (this.signalRService.connectionStarted === false) {
      this.signalRService.connected$.subscribe(() => this.subscribe());
      this.signalRService.startConnection();
    } else {
      this.subscribe();
    }
  }

  subscribe(): void {
    this.signalRService.addTransferChartDataListener('Pie', this.data$);
 }
}
