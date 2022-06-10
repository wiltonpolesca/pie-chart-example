import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  hubConnection?: signalR.HubConnection;
  connectionStarted: boolean = false;

  connected$ = new Subject<boolean>();

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.signalRURL)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Trace)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.connectionStarted = true;
        console.log('Connection started');
        this.connected$.next(true);
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public addTransferChartDataListener = (method: string, obs: Subject<any>) => {
    this.hubConnection?.on(method, (data) => {
      console.log({data});
      obs.next(data);
    });
  }
}
