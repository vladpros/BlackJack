import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { GetNameGameView } from '../models/get-name-game-view';
import { environment } from 'src/environments/environment';

@Injectable()

export class StartGameService {

  constructor(private httpClient: HttpClient) {
   }

   private controllerName = '/api/Game/';

   public getNames(): Observable<GetNameGameView> {
    return this.httpClient.get<GetNameGameView>(environment.webApiAdress + this.controllerName + 'GetName');
  }

  public StartGame(input: any): Observable<number> {
    return this.httpClient
    .get<number>(environment.webApiAdress + this.controllerName + 'StartGame?name=' + input.name + '&botsNumber=' + input.botsNumber);
  }


}
