import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { NameView } from '../models/name-view';
import { environment } from 'src/environments/environment';

@Injectable()

export class StartGameService {

  constructor(private httpClient: HttpClient) {
   }

   private controllerName = '/api/Game/';

  getNames(): Observable<NameView> {
    return this.httpClient.get<NameView>(environment.webApiAdress + this.controllerName + 'GetName');
  }

  StartGame(input): Observable<number> {
    return this.httpClient
    .get<number>(environment.webApiAdress + this.controllerName + 'StartGame?name=' + input.name + '&botsNumber=' + input.botsNumber);
  }


}
